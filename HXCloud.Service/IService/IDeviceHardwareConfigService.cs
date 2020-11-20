using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDeviceHardwareConfigService : IBaseService<DeviceHardwareConfigModel>
    {
        /// <summary>
        /// 添加设备PLC数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="DeviceSn">设备序列号</param>
        /// <param name="req">PLC参数</param>
        /// <returns></returns>
        Task<BaseResponse> AddDeviceHardwareConfigAsync(string Account, string DeviceSn, DeviceHardwareConfigAddDto req);
        /// <summary>
        /// 修改设备plc配置数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="DeviceSn">设备序列好</param>
        /// <param name="req">设备plc配置数据</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateDeviceHardwareConfigAsync(string Account, string DeviceSn, DeviceHardwareConfigUpdateDto req);
        /// <summary>
        /// 删除设备plc配置数据
        /// </summary>
        /// <param name="Id">设备PLC数据标示</param>
        /// <param name="account">操作人</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteDeviceHardwareConfigAsync(int Id, string account);
        /// <summary>
        /// 获取设备PLC配置数据
        /// </summary>
        /// <param name="Id">设备PLC数据标示</param>
        /// <returns></returns>
        Task<BaseResponse> GetHardwareConfigAsync(int Id);
        /// <summary>
        /// 获取设备plc配置数据
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <param name="req">分页参数</param>
        /// <returns></returns>
        Task<BaseResponse> GetTypeHardwareConfigAsync(string DeviceSn, BasePageRequest req);
    }
}
