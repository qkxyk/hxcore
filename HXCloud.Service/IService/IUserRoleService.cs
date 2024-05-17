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
        /// <summary>
        /// 根据用户标识获取用户角色(角色用逗号分割)
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <returns></returns>
        Task<string> GetUserRolesAsync(int userId);
    }
}
