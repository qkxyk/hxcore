using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IUserRoleRepository : IBaseRepository<UserRoleModel>
    {
        /// <summary>
        /// 根据用户标识获取用户的角色信息
        /// </summary>
        /// <param name="Id">用户标识</param>
        /// <returns></returns>
        Task<IEnumerable<UserRoleModel>> FindWithRole(int Id);
        Task SaveAsync(int UserId, List<int> RoleIds, string account);
    }
}
