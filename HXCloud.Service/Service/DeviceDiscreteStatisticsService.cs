using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class DeviceDiscreteStatisticsService : IDeviceDiscreteStatisticsService
    {
        private readonly IDeviceDiscreteStatisticsRepository _dsr;
        private readonly IMapper _map;

        public DeviceDiscreteStatisticsService(IDeviceDiscreteStatisticsRepository dsr, IMapper map)
        {
            this._dsr = dsr;
            this._map = map;
        }
        public Task<bool> IsExist(Expression<Func<DeviceDiscreteStatisticsDataModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取设备统计数据
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <param name="devices">设备编号列表，如果是单个设备，该值为空</param>
        /// <returns>返回设备的统计数据</returns>
        public async Task<BaseResponse> GetDeviceStatisticsAsync(DeviceStatisticsRequestDto req, List<string> devices)
        {
            List<DeviceDiscreteStatisticsDataModel> data;
            IQueryable<DeviceDiscreteStatisticsDataModel> query;
            if (req.IsDevice)
            {
                query =  _dsr.FindWithDevice(a => a.DeviceSn == req.DeviceSn);
            }
            else
            {
                query =  _dsr.FindWithDevice(a => devices.Contains(a.DeviceSn));
            }
            data = await query.Where(a => a.Date <= req.EndTime && a.Date >= req.BeginTime).ToListAsync();
            var dtos = _map.Map<List<DeviceStatisticsDto>>(data);
            return new BResponse<List<DeviceStatisticsDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
    }
}
