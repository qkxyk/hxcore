using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IUserRoleService : IBaseService<UserRoleModel>
    {
        Task<BaseResponse> AddUserRoleAsync(UserRoleAddViewModel req, string account);
        //HandleResponse<UserRoleKey> DeleteUserRole(UserRoleDeleteViewModel req);
        Task<BaseResponse> GetUserRole(int UserId);
    }
}
