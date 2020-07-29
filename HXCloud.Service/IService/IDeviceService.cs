using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDeviceService : IService<DeviceModel, DeviceCheckDto>
    {
        Task<BaseResponse> AddDeviceAsync(DeviceAddDto req, string account, string GroupId);
        Task<BaseResponse> UpdateDeviceAsync(string account, string GroupId, DeviceUpdateViewModel req);
        Task<BaseResponse> UpdateDeviceTypeAsync(string account, string DeviceSn, int TypeId);
    }
}
