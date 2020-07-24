using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IRoleService : IBaseService<RoleModel>
    {
        Task<BaseResponse> AddRoleAsync(RoleAddDto req, string account, string GroupId);
        Task<BaseResponse> UpdateRoleAsync(RoleUpdateDto req, string account, string GroupId);
        Task<BaseResponse> DeleteRoleAsync(int roleId, string account);
        Task<List<int>> GetRoles(Expression<Func<RoleModel, bool>> predicate);
        //bool IsExist(Expression<Func<RoleModel, bool>> Predicate);
        Task<BaseResponse> GetRoles(string GroupId, BasePageRequest req);
    }
}
