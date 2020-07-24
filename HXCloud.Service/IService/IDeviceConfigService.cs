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
    }
}
