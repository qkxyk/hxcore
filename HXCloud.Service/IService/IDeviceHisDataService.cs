using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDeviceHisDataService : IBaseService<DeviceHisDataModel>
    {
        Task<BaseResponse> GetDeviceHisDataAsync(string DeviceSn, DeviceHisDataPageRequest req);
        /// <summary>
        /// 获取设备的最新一条历史数据
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <returns>返回设备最新一条历史数据</returns>
        Task<BaseResponse> GetDeviceLatestHisDataAsync(string DeviceSn,int order);
    }
}
