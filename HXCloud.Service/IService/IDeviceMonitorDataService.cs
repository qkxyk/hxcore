using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IDeviceMonitorDataService:IBaseService<DeviceMonitorDataModel>
    {
        /// <summary>
        /// 获取设备数采仪数据
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <returns></returns>
        Task<BaseResponse> GetDeviceMonitorAsync(string DeviceSn, DeviceMonitorDataRequestDto req);
    }
}
