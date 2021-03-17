using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class PlcSecurityService : IPlcSecurityService
    {
        private readonly ILogger<PlcSecurityService> _log;
        private readonly IMapper _mapper;
        private readonly IPlcSecurityRepository _psr;

        public PlcSecurityService(ILogger<PlcSecurityService> log, IMapper map, IPlcSecurityRepository psr)
        {
            this._log = log;
            this._mapper = map;
            this._psr = psr;
        }

        /// <summary>
        /// 添加plc鉴权码，返回鉴权码
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">鉴权码数据</param>
        /// <returns>返回鉴权码</returns>
        public async Task<BaseResponse> AddPlcSecurityAsync(string Account, PlcSecurityAddDto req)
        {
            try
            {
                req.SecurityKey = CreateKey(req.SecurityKey);
                var entity = _mapper.Map<PlcSecurityModel>(req);
                entity.Create = Account;
                //entity.SecurityKey = CreateKey(req.SecurityKey);
                await _psr.AddAsync(entity);
                _log.LogInformation($"{Account}生成{entity.SecurityKey}PLC鉴权码成功");
                return new HandleResponse<string> { Success = true, Message = "生成PLC鉴权码成功", Key = entity.SecurityKey };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加PLC鉴权码失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "生成PLC鉴权码失败" };
            }
        }
        /// <summary>
        /// plc程序加密算法
        /// 加密分成三个阶段
        /// 第一阶段：鉴权码(6位)循环左移3位生成tmp1，tmp1和鉴权码或得到tmp2,tmp2与鉴权码相加得到tmp3
        ///第二阶段：取当前日期(日)，高低字节交换，生成tmp4，当tmp4<1000时乘以10011，否则乘以1001得到tmp5，tmp5加上tmp4得到tmp6
        /// 第三阶段：tmp3和tmp6或得到AKey
        /// </summary>
        /// <param name="key">PLC程序鉴权码</param>
        /// <returns>返回plc程序的密钥</returns>
        private string CreateKey(string key)
        {
            int num = int.Parse(key);
            //鉴权码左移三位
            var tmp1 = num << 3;
            //与鉴权码或
            var tmp2 = tmp1 | num;
            //与鉴权码相加
            var tmp3 = tmp2 + num;
            ushort date = (ushort)DateTime.Now.Date.Day;
            //高低字节交换
            var top = date >> 8;
            var sh = date << 8;
            var tmp4 = sh | top;
            //判断乘以10011或是1001
            var tmp5 = tmp4 >= 1000 ? tmp4 * 1001 : tmp4 * 10011;

            var tmp6 = tmp5 + tmp4;
            var AKey = tmp3 | tmp6;
            return AKey.ToString();
        }
    }

}

