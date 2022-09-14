using HXCloud.Model;
using HXCloud.ViewModel;
using HXCloud.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IUserService : IBaseService<UserModel>
    {
        string GetUserName();
        Task<BaseResponse> GetUserInfoAsync(int Id, bool IsAdmin);
        Task<BaseResponse> UserLoginAsync(LoginViewModel req);
        Task<BaseResponse> UserLoginJwtAsync(LoginViewModel req);
        //bool IsExist(Expression<Func<UserModel, bool>> predicate);
        Task<BaseResponse> UserRegisterAsync(UserRegisterViewModel req);
        Task<BaseResponse> UpdateUserImageAsync(int Id, string account, string filePath);
        Task<BaseResponse> UpdateUserInfoAsync(string account, UserUpdateViewModel req, int Id);

        Task<BaseResponse> GetUsersAsync(UserPageRequestViewModel req, string GroupId);
        Task<BaseResponse> GetUserAsync(int id);
        Task<string> GetUserGroupAsync(int Id);

        Task<BaseResponse> UpdateUserStatusAsync(UserStatusUpDateViewModel req, string account);
        Task<BaseResponse> DeleteUserAsync(int Id, string account);
        Task<BaseResponse> UpdateUserPasswordAsync(UserPasswordViewModel req, int Id);
        Task<BaseResponse> ResetPassword(UserResetPasswordViewModel req, string account);
        Task<BaseResponse> AddUserAsync(string Account, string GroupId, UserAddViewModel req);

        /// <summary>
        /// 获取用户名列表（用户的下级，用于运维平台）
        /// </summary>
        /// <param name="Account">用户名</param>
        ///<param name="isAdmin">是否管理员，管理员能看到所有巡检人员填写的巡检单</param>
        /// <returns>返回用户列表,如果用户不存在返回空数据</returns>
        Task<Dictionary<string, UserCategory>> GetUserAndChildAsync(string Account, bool isAdmin);
        /// <summary>
        /// 修改用户的运维信息
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">运维数据</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateUserOpsAsync(string account, UserOpsUpdateDto req);
        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <param name="account">用户名称</param>
        /// <returns>用户数据</returns>
        Task<UserData> GetUserByAccountAsync(string account);
    }
}
