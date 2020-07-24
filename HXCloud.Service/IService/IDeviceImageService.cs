using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDeviceImageService:IBaseService<DeviceImageModel>
    {
        Task<BaseResponse> AddDeviceImageAsync(string account, DeviceImageAddDto req, string deviceSn, string path);
        Task<BaseResponse> UpdateDeviceImageAsync(string account, DeviceImageUpdateDto req, string deviceSn);
        Task<BaseResponse> DeleteDeviceImageAsync(string account, int Id, string path);
        Task<BaseResponse> GetDeviceImageAsync(int Id);
        Task<BaseResponse> GetAllDeviceImageAsync(string deviceSn);
    }
}
