using AutoMapper;
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
    public class WarnService : IWarnService
    {
        private readonly ILogger<WarnService> _log;
        private readonly IMapper _mapper;
        private readonly IWarnRepository _warn;
        private readonly IWarnTypeRepository _wt;

        public WarnService(ILogger<WarnService> log, IMapper mapper, IWarnRepository warn, IWarnTypeRepository wt)
        {
            this._log = log;
            this._mapper = mapper;
            this._warn = warn;
            this._wt = wt;
        }
        public Task<bool> IsExist(Expression<Func<WarnModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public async Task<BaseResponse> GetWarnById(int Id)
        {
            var data = await _warn.FindWithCodeAndType(a => a.Id == Id).FirstOrDefaultAsync();
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的报警编号不存在" };
            }
            var dto = _mapper.Map<WarnDto>(data);
            return new BResponse<WarnDto> { Success = true, Message = "获取数据成功", Data = dto };
        }

        /// <summary>
        /// 获取设备的报警，没有输入时间，则时间为昨天到今天的时间,搜索条件为Code
        /// </summary>
        /// <param name="deviceSn"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetWarnByDeviceSnAsync(string deviceSn, DeviceWarnPageRequest req)
        {
            var data = _warn.FindWithCodeAndType(a => a.DeviceSn == deviceSn);
            if (req.BeginTime == null)
            {
                req.BeginTime = DateTime.Now.AddDays(-1);
            }
            if (req.EndTime == null)
            {
                req.EndTime = DateTime.Now;
            }
            if (req.BeginTime >= req.EndTime)
            {
                return new BaseResponse { Success = false, Message = "开始时间不能大于或者等于结束时间" };
            }
            data = data.Where(a => a.Dt > req.BeginTime && a.Dt < req.EndTime);
            if (req.State != 0)
            {
                data = data.Where(a => a.State == true);
            }
            else
            {
                data = data.Where(a => a.State == false);
            }
            int count = data.Count();
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => a.Code == req.Search);
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
            var ret = await data.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<WarnDto>>(ret);
            var br = new BasePageResponse<List<WarnDto>>();
            br.Success = true;
            br.Message = "获取数据成功";
            br.PageSize = req.PageSize;
            br.CurrentPage = req.PageNo;
            br.Count = count;
            br.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            br.Data = dtos;
            return br;
        }


        ///// <summary>
        ///// 获取项目或者场站的报警数据,需在Control中获取设备编号列表
        /// </summary>
        /// <param name="Devices">设备序列号列表</param>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="state">报警状态</param>
        /// <param name="req">分页基本信息</param>
        /// <returns>返回项目获取场站报警分页数据</returns>
        public async Task<BaseResponse> GetProjectWarnAsync(List<string> Devices, DateTime begin, DateTime end, bool state, BasePageRequest req)
        {
            var data = _warn.FindWithCodeAndType(a => Devices.Contains(a.DeviceSn) && a.Dt > begin && a.Dt < end && a.State == state);
            int count = data.Count();
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => a.Code == req.Search);
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
            var ret = await data.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<WarnDto>>(ret);
            var br = new BasePageResponse<List<WarnDto>>();
            br.Success = true;
            br.Message = "获取数据成功";
            br.PageSize = req.PageSize;
            br.CurrentPage = req.PageNo;
            br.Count = count;
            br.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            br.Data = dtos;
            return br;
        }

        /// <summary>
        /// 统计未处理的报警数据
        /// </summary>
        /// <param name="Devices">列表序列号列表</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetWarnStatisticsAsync(List<string> Devices)
        {
            var types = await _wt.FindTypesWithCodeAsync(a => true);
            List<WarnTypeStatisticsDataDto> list = new List<WarnTypeStatisticsDataDto>();
            foreach (var item in types)
            {
                var codes = item.WarnCode.Select(a => a.Code).ToList();
                var warns = _warn.Find(a => codes.Contains(a.Code) && a.State == false && Devices.Contains(a.DeviceSn)).Select(a => a.DeviceSn).Distinct().Count();
                list.Add(new WarnTypeStatisticsDataDto { Id = item.Id, Name = item.TypeName, Num = warns });
            }
            return new BResponse<List<WarnTypeStatisticsDataDto>> { Success = true, Message = "获取数据成功", Data = list };
        }
        public async Task<BaseResponse> UpdateWarnInfo(string account, WarnUpdateDto req)
        {
            var warn = await _warn.FindAsync(req.Id);
            if (warn == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据编号不存在" };
            }
            try
            {
                warn.State = true;
                warn.Modify = account;
                warn.ModifyTime = DateTime.Now;
                warn.Comments = req.Comments;
                await _warn.SaveAsync(warn);
                _log.LogInformation($"{account}处理了标识未{req.Id}的报警信息");
                return new BaseResponse { Success = true, Message = "处理数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}处理标识为{req.Id}的报警信息失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "处理数据失败，请联系管理员" };
            }
        }
    }
}
