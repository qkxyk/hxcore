using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDeviceConfigService : IBaseService<DeviceConfigModel>
    {
        Task<BaseResponse> AddDeviceConfigAsync(string account, DeviceConfigAddDto req, string deviceSn);
        Task<BaseResponse> UpdateDeviceConfigAsync(string account, DeviceConfigUpdateDto req, string deviceSn);
        Task<BaseResponse> DeleteDeviceConfigAsync(string account, int Id);
        Task<BaseResponse> GetDeviceConfigAsync(int Id);
        Task<BaseResponse> GetDeviceConfigsAsync(string deviceSn);
        /// <summary>
        /// 用于httppath部分修改数据
        /// </summary>
        /// <param name="Id">设备配置标识</param>
        /// <returns></returns>
        Task<DeviceConfigDto> GetDeviceConfigByIdAsync(int Id);
        /// <summary>
        /// 修改设备配置数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">修改数据</param>
        /// <returns></returns>
        Task<BaseResponse> PatchDeviceConfigAsync(string Account, DeviceConfigDto req);
    }
}
