using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IRoleProjectRepository : IBaseRepository<RoleProjectModel>
    {
        /// <summary>
        /// 修改角色的项目权限
        /// </summary>
        /// <param name="RoleId">角色标示</param>
        /// <param name="rp">角色的项目权限列表</param>
        /// <returns></returns>
        Task SaveAsync(int RoleId, List<RoleProjectModel> rp);
    }
}
