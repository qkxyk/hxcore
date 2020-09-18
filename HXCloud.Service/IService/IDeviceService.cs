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
        Task<BaseResponse> ChangeDeviceProject(string account, string DeviceSn, string GroupId, int? projectId);
        Task<BaseResponse> GetProjectDeviceAsync(string GroupId, int projectId, bool isSite, BasePageRequest req);
        Task<BaseResponse> GetMyDevice(string GroupId, string roles, bool isAdmin, BasePageRequest req);
        Task<List<string>> GetProjectDeviceSnAsync(int projectId, bool isSite);
        /// <summary>
        /// 获取我的设备编号
        /// </summary>
        /// <param name="GroupId">组织编号</param>
        /// <param name="roles">我的角色</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <returns>返回我的设备编号列表</returns>
        Task<List<string>> GetMyDeviceSnAsync(string GroupId, string roles, bool isAdmin);
        /// <summary>
        /// 获取场站列表下的所有设备
        /// </summary>
        /// <param name="Sites">场站列表</param>
        /// <returns>返回所有设备</returns>
        Task<BaseResponse> GetAllDeviceAsync(List<int> Sites);
    }
}
