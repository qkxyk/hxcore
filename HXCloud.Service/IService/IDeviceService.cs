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
        /// <summary>
        /// 获取无项目的设备
        /// </summary>
        /// <param name="GroupId">组织编号</param>
        /// <param name="req">分页参数</param>
        /// <returns>返回无项目设备列表</returns>
        Task<BaseResponse> GetNoProjectDeviceAsync(string GroupId, BasePageRequest req);

        /// <summary>
        /// 获取设备的总揽数据
        /// </summary>
        /// <param name="sites">场站编号集合</param>
        /// <returns></returns>
        Task<BaseResponse> GetDeviceOverViewAsync(List<int> sites, string GroupId);

        /// <summary>
        /// 获取场站列表下的所有设备编号
        /// </summary>
        /// <param name="sites">场站列表</param>
        /// <returns>返回设备编号列表</returns>
        Task<List<string>> GetDeviceSnBySitesAsync(List<int> sites);

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="DeviceSn">设备编号</param>
        /// <param name="path">设备图片保存路径</param>
        /// <returns>返回删除设备是否成功</returns>
        Task<BaseResponse> DeleteDeviceAsync(string Account, string DeviceSn, string path);

        /// <summary>
        /// 转换设备数据为设备修改数据
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <returns>返回设备修改数据</returns>
        Task<DevicePatchDto> GetDevicePathDtoAsync(string DeviceSn);
        /// <summary>
        /// 部分更新设备数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="DeviceSn">设备编号</param>
        /// <param name="req">要修改的数据</param>
        /// <returns>返回修改设备是否成功</returns>
       Task<BaseResponse> PathUpdateDeviceAsync(string Account, string DeviceSn, DevicePatchDto req);
    }
}
