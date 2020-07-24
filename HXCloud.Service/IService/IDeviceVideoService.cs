using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDeviceVideoService : IBaseService<DeviceVideoModel>
    {
        Task<BaseResponse> AddDeviceVideoAsync(string account, DeviceVideoAddDto req, string deviceSn);
        Task<BaseResponse> UpdateDeviceVideoAsync(string account, DeviceVideoUpdateDto req, string deviceSn);
        Task<BaseResponse> DeleteDeviceVideoAsync(string account, int Id);
        Task<BaseResponse> GetDeviceVideoAsync(int Id);
        Task<BaseResponse> GetDeviceVideoesAsync(string deviceSn);
    }
}
