using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IOpsStatisticsService
    {
        Task<BaseResponse> GetOpsStatisticsAsync(List<string> users, OpsStatisticsRequest req);
        /// <summary>
        ///获取运维统计数据，完成的只统计本月数据，未完成的统计全部 
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <param name="account">非管理员没有查询权限的查找自己</param>
        /// <param name="DeviceSn">非管理员有查询权限查看的设备列表</param>
        /// <returns></returns>
        Task<BaseResponse> GetOpsStatisticsAsync(OpsStatisticsRequest req, bool isAdmin, string account, List<string> DeviceSn);
    }
}
