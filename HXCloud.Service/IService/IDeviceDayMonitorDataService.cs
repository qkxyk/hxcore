using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDeviceDayMonitorDataService : IBaseService<DeviceDayMonitorDataModel>
    {
        /// <summary>
        /// 获取设备数采仪数据
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <returns></returns>
        Task<BaseResponse> GetDeviceMonitorAsync(string DeviceSn, DeviceMonitorDataRequestDto req);
    }
}
