using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDeviceMigrationService : IBaseService<DeviceMigrationModel>
    {
        /// <summary>
        /// 获取设备的迁移记录
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <returns>返回设备的迁移记录</returns>
        Task<BaseResponse> GetDeviceMigrationAsync(string DeviceSn);
    }
}
