using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Service
{
    public class DeviceLogService : IDeviceLogService
    {
        private readonly IDeviceLogRepository _dlr;
        private readonly IMapper _mapper;
        private readonly ILogger<DeviceLogService> _log;
        private readonly ITypeDataDefineRepository _tdd;//类型数据定义数据

        public DeviceLogService(IDeviceLogRepository dlr, IMapper mapper, ILogger<DeviceLogService> log, ITypeDataDefineRepository tdd)
        {
            this._dlr = dlr;
            this._mapper = mapper;
            this._log = log;
            this._tdd = tdd;
        }
        /// <summary>
        /// 写入设备控制日志
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="DeviceSn">设备序列号</param>
        /// <param name="req">设备操作日志</param>
        /// <returns>返回写入设备日志是否成功</returns>
        public async Task<BaseResponse> AddDeviceLogAsync(string Account, string DeviceSn, int TypeId, DeviceLogAddDto req)
        {
            try
            {
                var entity = _mapper.Map<DeviceLogModel>(req);
                entity.Create = Account;//注，创建者和操作者重复
                entity.Account = Account;
                entity.DeviceSn = DeviceSn;
                //获取数据key对应的keyname
                var name = (await _tdd.Find(a => a.DataKey == req.Key && a.TypeId == TypeId).FirstOrDefaultAsync()).DataName;
                entity.KeyName = name;
                await _dlr.AddAsync(entity);
                _log.LogInformation($"{Account}写入设备{DeviceSn}控制日志成功");
                return new HandleResponse<int> { Success = true, Message = "写入日志成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}写入设备{DeviceSn}控制日志失败,失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "写入日志失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 删除设备操作日志
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="Id">操作日志编号</param>
        /// <returns>返回是否删除成功</returns>
        public async Task<BaseResponse> RemoveDeviceLogAsync(string Account, int Id)
        {
            var entity = await _dlr.FindAsync(Id);
            if (entity == null)
            {
                return new BaseResponse { Success = false, Message = "输入的编号不存在" };
            }
            try
            {
                await _dlr.RemoveAsync(entity);
                _log.LogInformation($"{Account}删除编号为{Id}的设备操作日志成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}删除编号为{Id}的设备操作日志失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除设备操作日志失败，请联系管理员" };
            }
        }

        /// <summary>
        /// 获取设备分页操作日志
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <param name="req">请求参数</param>
        /// <returns>返回分页数据</returns>
        public async Task<BaseResponse> GetDeviceLogsAsync(string DeviceSn, DeviceLogPageRequest req)
        {
            var data = _dlr.Find(a => a.DeviceSn == DeviceSn);
            if (req.BeginTime == null)
            {
                req.BeginTime = DateTime.Now.AddMonths(-1);
            }
            if (req.EndTime == null)
            {
                req.EndTime = DateTime.Now;
            }
            if (req.BeginTime >= req.EndTime)
            {
                return new BaseResponse { Success = false, Message = "开始时间不能大于或者等于结束时间" };
            }
            data = data.Where(a => a.CreateTime > req.BeginTime && a.CreateTime < req.EndTime);
            int count = data.Count();
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => a.KeyName == req.Search || a.Key == req.Search);
            }
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            //var ret = await data.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var retData = await data.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dy = _dlr.GetWithUserNameAsync(retData);
            //List<DeviceLogData> list = new List<DeviceLogData>();
            var br = new BasePageResponse<IEnumerable<DeviceLogData>>();
            br.Data = dy;
            br.Success = true;
            br.Message = "获取数据成功";
            br.PageSize = req.PageSize;
            br.CurrentPage = req.PageNo;
            br.Count = count;
            br.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            //br.Data = dtos;
            return br;
        }

        public Task<bool> IsExist(Expression<Func<DeviceLogModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public class TestData
        {
            public string UserName { get; set; }
            public DeviceLogModel Log { get; set; }
        }
    }
}
