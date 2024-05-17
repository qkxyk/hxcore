using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IRoleProjectService : IBaseService<RoleProjectModel>
    {
        Task<bool> IsAuth(string roles, string pathId, int operate);
        /// <summary>
        /// 更改角色项目全新啊
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="RoleId">角色标示</param>
        /// <param name="projects">项目或者场站列表</param>
        /// <param name="operate">操作</param>
        /// <returns></returns>
        Task<BaseResponse> AddOrUpdateRoleProjectAsync(string Account, int RoleId, int[] projects, int[] operate);
        /// <summary>
        /// 获取角色项目
        /// </summary>
        /// <param name="roleId">金额us标示</param>
        /// <returns>角色项目列表</returns>
        Task<BaseResponse> GetRoleProjectAsync(int roleId);
        /// <summary>
        /// 根据角色列表获取角色的项目和场站
        /// </summary>
        /// <param name="Roles">角色列表</param>
        /// <returns></returns>
        Task<List<int>> GetRoleSitesAsync(List<int> Roles);
    }
}
