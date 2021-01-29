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
        Task<BaseResponse> DeleteDeviceCardAsync(string account, int Id);
        //Task<BaseResponse> GetDeviceCardAsync(string cardNo);
        Task<BaseResponse> GetDeviceCardsAsync(string deviceSn);
        /// <summary>
        /// 更新流量卡的定位、IMEI和ICCID数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="DeviceSn">设备序列号</param>
        /// <param name="req">流量卡的定位等信息</param>
        /// <returns>返回是否更新成功</returns>
        Task<BaseResponse> UpdateDeviceCardPositionAsync(string account, string DeviceSn, DeviceCardPositionUpdateDto req);
    }
}
