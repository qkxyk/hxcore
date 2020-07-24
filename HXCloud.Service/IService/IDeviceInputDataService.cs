using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDeviceInputDataService : IBaseService<DeviceInputDataModel>
    {
        Task<BaseResponse> AddDeviceInputDataAsync(string account, DeviceInputAddDto req, string deviceSn);
        Task<BaseResponse> UpdateDeviceInputDataAsync(string account, DeviceInputDataUpdateDto req, string deviceSn);
        Task<BaseResponse> DeleteDeviceInputDataAsync(string account, int Id);
        Task<BaseResponse> GetDeviceInputDataAsync(int Id);
        Task<BaseResponse> GetAllDeviceInputDataAsync(string deviceSn);
    }
}
