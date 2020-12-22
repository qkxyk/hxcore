using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDeviceStatistcsDataService : IBaseService<DeviceStatisticsDataModel>
    {
        /// <summary>
        /// 获取设备统计数据
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <param name="devices">设备编号列表，如果是单个设备，该值为空</param>
        /// <returns>返回设备的统计数据</returns>
        Task<BaseResponse> GetDeviceStatisticsAsync(DeviceStatisticsRequestDto req, List<string> devices);
    }
}
