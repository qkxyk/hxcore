using AutoMapper;
using HXCloud.Common;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class BoxService : IBoxService
    {
        private readonly IBoxRepository _box;
        private readonly ILogger<BoxService> _log;
        private readonly IMapper _mapper;

        public BoxService(IBoxRepository box, ILogger<BoxService> log, IMapper mapper)
        {
            this._box = box;
            this._log = log;
            this._mapper = mapper;
        }
        public Task<bool> IsExist(Expression<Func<BoxModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        //添加盒子
        public async Task<BaseResponse> AddBoxAsync(string account, BoxAddDto req)
        {
            //检查是否存在
            var ext = await _box.Find(a => a.UUId == req.UUId).FirstOrDefaultAsync();
            if (ext != null)
            {
                return new BaseResponse { Success = false, Message = "输入的盒子已存在" };
            }
            try
            {
                var entity = _mapper.Map<BoxModel>(req);
                entity.Create = account;
                await _box.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}盒子成功");
                return new HandleResponse<int> { Success = true, Message = "添加盒子成功", Key = entity.Id };
            }
            catch (Exception ex)
            {

                _log.LogError($"{account}添加uuid为：{req.UUId}的盒子失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加盒子失败" };
            }
        }
        //批量添加盒子

        //删除盒子

        public async Task<BaseResponse> DeleteBoxAsync(string account, int Id)
        {
            var ret = await _box.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的盒子编号不存在" };
            }
            try
            {
                await _box.RemoveAsync(ret);
                _log.LogInformation($"{account}删除标识为:{Id}的盒子成功");
                return new BaseResponse { Success = true, Message = "删除盒子成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标识为:{Id}的盒子失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除盒子失败，请联系管理员" };
            }
        }
        //返回盒子信息
        public async Task<BaseResponse> GetBoxAsync(int Id)
        {
            var ret = await _box.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = $"没有标识为{Id}盒子" };
            }
            var dto = _mapper.Map<BoxDto>(ret);
            var rm = new BResponse<BoxDto> { Success = true, Message = "获取数据成功", Data = dto };
            return rm;
        }
        //返回盒子列表
        public async Task<BaseResponse> GetPageBoxAsync(BasePageRequest req)
        {
            var query = _box.Find(a => true == true);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                query = query.Where(a => a.UUId.Contains(req.Search));
            }
            int Count = query.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
                //UserQuery = UserQuery.OrderBy(a => a.Id);
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var data = await query.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<BoxDto>>(data);
            return new BasePageResponse<List<BoxDto>>
            {
                Success = true,
                Message = "获取数据成功",
                Count = Count,
                CurrentPage = req.PageNo,
                PageSize = req.PageSize,
                TotalPage = (int)Math.Ceiling((decimal)Count / req.PageSize),
                Data = dtos
            };
        }

        //盒子加密信息
        /// <summary>
        /// imei为第一次加密的key，uuid为第二次加密的key
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="serial"></param>
        /// <param name="imei"></param>
        public async Task<BaseResponse> EncryptDataAsync(string uuid, string serial, string imei)
        {
            var ext = await _box.Find(a => a.UUId == uuid).FirstOrDefaultAsync();
            if (ext == null)
            {
                return new BaseResponse { Success = false, Message = "输入的uuid不存在" };
            }
            NewTea tea = new NewTea();
            byte[] byContent = tea.strToToHexByte(serial);
            var ByKey = GetKeyBytes(imei);
            if (ByKey == null)
            {
                return new BaseResponse { Success = false, Message = "imei不正确" };
            }
            var byFirstResult = tea.EncryptByte(byContent, ByKey, true);
            string strFirst = tea.byteToHexStr(byFirstResult);
            var bySecondKey = GetKeyBytes(uuid);
            if (bySecondKey == null)
            {
                return new BaseResponse { Success = false, Message = "uuid不正确" };
            }
            var bySecondResult = tea.EncryptByte(byFirstResult, bySecondKey, true);
            string strSecondResult = tea.byteToHexStr(bySecondResult);
            try
            {
                //记录盒子激活信息
                ext.Activate = true;
                ext.Num += 1;
                await _box.SaveAsync(ext);
                _log.LogInformation($"激活标识为:{ext.Id},uuid为{ext.UUId}的盒子成功");
            }
            catch (Exception ex)
            {
                _log.LogInformation($"激活标识为:{ext.Id},uuid为{ext.UUId}的盒子失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
            }
            return new BResponse<string> { Success = true, Message = "获取密钥成功", Data = strSecondResult };

        }
        /// <summary>
        /// 返回16位长度的key
        /// </summary>
        /// <param name="strKey">key字符串</param>
        /// <returns></returns>
        byte[] GetKeyBytes(string strKey)
        {
            byte[] byKey = new byte[16];
            int length = strKey.Length;
            byte[] by = Encoding.UTF8.GetBytes(strKey);
            if (length < 15)
            {
                return null;
            }
            else if (length == 15)
            {
                //按位异或
                int cal = 0;
                for (int i = 0; i < 15; i++)
                {
                    cal ^= by[i];
                }
                Array.Copy(by, 0, byKey, 0, 15);
                byKey[15] = (byte)cal;
            }
            else
            {
                Array.Copy(by, 0, byKey, 0, 16);
            }
            return byKey;
        }
    }
}
