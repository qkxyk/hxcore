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
            string tableName = $"devhis_{req.Date.ToString("yyyyMMdd")}";
            var ret = await _dhr.CheckTableExistAsyn(tableName);
            if (!ret)
            {
                return new BaseResponse { Success = false, Message = "该日期内没有数据" };
            }
            var data = await _dhr.FindDeviceHisDataAsync(tableName, DeviceSn);
            //if (!string.IsNullOrWhiteSpace(req.Search))
            //{
            //    data = data.Where(a => a.Name.Contains(req.Search));
            //}
            var dtos = _mapper.Map<List<DeviceHisDataDto>>(data);
            var br = new BResponse<List<DeviceHisDataDto>>()
            {
                Success = true,
                Message = "获取数据成功",
                Data = dtos
            };
            return br;
        }

        /// <summary>
        /// 获取设备的最新一条历史数据
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <returns>返回设备最新一条历史数据</returns>
        public async Task<BaseResponse> GetDeviceLatestHisDataAsync(string DeviceSn,int order)
        {
            string tableName = $"devhis_{DateTime.Now.ToString("yyyyMMdd")}";
            var ret = await _dhr.CheckTableExistAsyn(tableName);
            if (!ret)
            {
                return new BaseResponse { Success = false, Message = "该日期内没有数据" };
            }
            string orderType = "desc";
            if (order==1)
            {
                orderType = "asc";
            }
            var data = await _dhr.FindFirstOrDefaultAsync(tableName, DeviceSn,orderType);
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
