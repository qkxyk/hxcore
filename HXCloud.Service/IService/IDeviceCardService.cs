using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDeviceCardService : IBaseService<DeviceCardModel>
    {
        Task<BaseResponse> AddDeviceCardAsync(string DeviceSn, DeviceCardAddDto req, string account);
        Task<BaseResponse> UpdateDeviceCardAsync(string account, DeviceCardUpdateDto req, string DeviceSn);
        Task<BaseResponse> DeleteDeviceCardAsync(string account, string cardNo);
        Task<BaseResponse> GetDeviceCardAsync(string cardNo);
        Task<BaseResponse> GetDeviceCardsAsync(string deviceSn);
    }
}
