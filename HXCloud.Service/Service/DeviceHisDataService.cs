using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Service
{
    public class DeviceHisDataService : IDeviceHisDataService
    {
        private readonly IDeviceHisDataRepository _dhr;
        private readonly IMapper _mapper;

        public DeviceHisDataService(IDeviceHisDataRepository dhr, IMapper mapper)
        {
            this._dhr = dhr;
            this._mapper = mapper;
        }
        public Task<bool> IsExist(Expression<Func<DeviceHisDataModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> GetDeviceHisDataAsync(string DeviceSn, DeviceHisDataPageRequest req)
        {
            var data = _dhr.FindWithDevice(a => a.DeviceSn == DeviceSn);
            //if (!string.IsNullOrWhiteSpace(req.Search))
            //{
            //    data = data.Where(a => a.Name.Contains(req.Search));
            //}
            data = data.Where(a => a.Dt > req.Begin && a.Dt < req.End);
            int count = data.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "dt desc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var list = await data.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();

            var dtos = _mapper.Map<List<DeviceHisDataDto>>(list);
            var br = new BasePageResponse<List<DeviceHisDataDto>>()
            {
                Success = true,
                Message = "获取数据成功",
                PageSize = req.PageSize,
                CurrentPage = req.PageNo,
                Count = count,
                TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize),
                Data = dtos
            };
            return br;
        }

        /// <summary>
        /// 获取设备的最新一条历史数据
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <returns>返回设备最新一条历史数据</returns>
        public async Task<BaseResponse> GetDeviceLatestHisDataAsync(string DeviceSn)
        {
            var data = await _dhr.FindWithDevice(a => a.DeviceSn == DeviceSn).OrderByDescending(a => a.Dt).FirstOrDefaultAsync();
            var dto = _mapper.Map<DeviceHisDataDto>(data);
            var br = new BResponse<DeviceHisDataDto>()
            {
                Success = true,
                Message = "获取数据成功",
                Data = dto
            };
            return br;
        }
    }
}
