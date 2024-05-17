using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IRoleModuleOperateService : IBaseService<RoleModuleOperateModel>
    {
        /// <summary>
        /// 添加角色模块操作
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<BaseResponse> AddRoleModuleOperateAsync(string account, RoleModuleOperateAddDto req);
        /// <summary>
        /// 删除角色模块操作
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<BaseResponse> DeleteRoleModuleOperateAsync(string account, int RoleId, int OperateId);
        /// <summary>
        /// 获取角色分配的操作
        /// </summary>
        /// <param name="RoleId">角色标识</param>
        /// <returns></returns>
        Task<BaseResponse> GetRoleOperatesAsync(int RoleId);
        /// <summary>
        /// 根据操作标识获取分配的角色列表
        /// </summary>
        /// <param name="OperateId">操作标识</param>
        /// <returns></returns>
        Task<List<int>> GetModuleOperatesAsync(int OperateId);
    }
}
