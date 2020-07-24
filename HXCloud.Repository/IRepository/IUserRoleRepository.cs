using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IUserRoleRepository : IBaseRepository<UserRoleModel>
    {
        Task<IEnumerable<UserRoleModel>> FindWithRole(int Id);
        Task SaveAsync(int UserId, List<int> RoleIds, string account);
    }
}
