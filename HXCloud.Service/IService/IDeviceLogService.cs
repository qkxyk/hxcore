using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDeviceLogService : IBaseService<DeviceLogModel>
    {
        /// <summary>
        /// 获取设备分页操作日志
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <param name="req">请求参数</param>
        /// <returns>返回分页数据</returns>
        Task<BaseResponse> GetDeviceLogsAsync(string DeviceSn, DeviceLogPageRequest req);
        /// <summary>
        /// 写入设备控制日志
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="DeviceSn">设备序列号</param>
        /// <param name="req">设备操作日志</param>
        /// <returns>返回写入设备日志是否成功</returns>
        Task<BaseResponse> AddDeviceLogAsync(string Account, string DeviceSn, DeviceLogAddDto req);
        /// <summary>
        /// 删除设备操作日志
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="Id">操作日志编号</param>
        /// <returns>返回是否删除成功</returns>
        Task<BaseResponse> RemoveDeviceLogAsync(string Account, int Id);
    }
}
