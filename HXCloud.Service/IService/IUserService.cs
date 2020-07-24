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
        Task<BaseResponse> GetUserInfoAsync(int Id);
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
    }
}
