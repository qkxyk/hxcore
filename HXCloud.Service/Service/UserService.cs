using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Common;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserStatus = HXCloud.Model.UserStatus;

namespace HXCloud.Service
{
    public class UserService : IUserService
    {
        private IUserRepository _user;
        private IGroupRepository _group;
        private readonly IUserRoleRepository _userRole;
        private readonly IMapper _mapper;
        private ILogger _log;
        public UserService(IUserRepository user, IGroupRepository group, IUserRoleRepository userRole, IMapper mapper, ILogger<UserService> log)
        {
            _user = user;
            _group = group;
            _userRole = userRole;
            this._mapper = mapper;
            _log = log;
        }
        public string GetUserName()
        {
            _log.LogInformation("this is from server log");
            //var m = _user.Find(a => a.Account == "xzc");
            //int count = m.Count();
            return "张三";
        }

        public async Task<BaseResponse> GetUserInfoAsync(int Id, bool IsAdmin)
        {
            var User = await _user.FindWithGroup(a => a.Id == Id);
            var dto = _mapper.Map<UserInfoDto>(User);
            dto.IsAdmin = IsAdmin;
            return new BResponse<UserInfoDto> { Success = true, Message = "获取数据成功", Data = dto };
        }

        public async Task<BaseResponse> GetUserAsync(int id)
        {
            var userInfo = await Task.Run(() =>
               {
                   var user = _user.Find(id);
                   if (user == null)
                   {
                       return new BaseResponse { Success = false, Message = "该用户不存在" };
                   }
                   var UserDto = _mapper.Map<UserData>(user);
                   var br = new BResponse<UserData>();
                   br.Data = UserDto;
                   br.Success = true;
                   br.Message = "获取用户数据成功";
                   return br;
               });
            return userInfo;
        }
        public async Task<BaseResponse> GetUsersAsync(UserPageRequestViewModel req, string GroupId)
        {
            var UserQuery = _user.Find(a => a.GroupId == GroupId);
            if (req.Status.HasValue)
            {
                UserQuery = UserQuery.Where(a => a.Status == (UserStatus)req.Status);
            }
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                UserQuery = UserQuery.Where(a => a.UserName == req.Search || a.Account == req.Search);
            }
            int count = UserQuery.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
                //UserQuery = UserQuery.OrderBy(a => a.Id);
            }
            else
            {
                var orderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var userList = await UserQuery.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dto = _mapper.Map<List<UserData>>(userList);
            var ret = new BasePageResponse<List<UserData>>()
            {
                Count = count,
                CurrentPage = req.PageNo,
                PageSize = req.PageSize,
                Success = true,
                Message = "获取数据成功",
                Data = dto,
                TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize)
            };
            return ret;
        }

        public async Task<BaseResponse> UserLoginAsync(LoginViewModel req)
        {
            BaseResponse br = new BaseResponse();
            UserMessage rm = new UserMessage();
            var UserInfo = await _user.FindWithGroup(a => a.Account == req.Account);
            if (UserInfo == null)//用户名不存在
            {
                br.Success = false;
                br.Message = "用户名不存在";
                return br;
            }
            string p = EncryptData.EncryptPassword(req.Password, UserInfo.Salt);
            if (p != UserInfo.Password)       //密码不正确
            {
                br.Success = false;
                br.Message = "密码不正确";
                return br;
            }
            if (UserInfo.Status == UserStatus.InActive)
            {
                br.Success = false;
                br.Message = "该账号未激活，请联系管理员激活";
                return br;
            }
            if (UserInfo.Status == UserStatus.InValid)
            {
                br.Success = false;
                br.Message = "该账号为无效账号";
                return br;
            }

            if (UserInfo.GroupId == null)     //用户没有加入组织
            {
                br.Success = false;
                br.Message = "用户没有加入组织，不能登录";
                return br;
            }
            //else
            //{
            //    rm.GroupId = UserInfo.GroupId;
            //    rm.GroupName = UserInfo.Group.GroupName;
            //}
            rm = _mapper.Map<UserMessage>(UserInfo);
            //获取用户的角色
            var r = await _userRole.FindWithRole(UserInfo.Id);
            if (r != null)
            {
                rm.Roles = r.Select(a => a.RoleId).ToString();
                foreach (var item in r)
                {
                    if (item.Role.IsAdmin)
                    {
                        rm.IsAdmin = true;
                        break;
                    }
                }
            }
            rm.Dt = DateTime.Now;
            string strUser = JsonConvert.SerializeObject(rm);
            br = new UserloginResponse() { Success = true, Message = "用户登录成功", Token = DesEncrypt.EncryptString(strUser) };
            return br; //new UserloginResponse() { Success = true, Message = "dfdfdfdsd", Token = @"B13F8398D170A45F0475461C9422629F2069ED228911C87A669C8F16AE78524620F5348EB0261C5ADBF35C761A65ADDF74B0D7800241F4B8346DDEE16E40E3F548DC9D9F63BCDB24FB0A0AB33AC858C8F4718F9A4134D28F155C8EDAC2909FAD30B2CCB4782D3E62EEB05101F767BC5C9EEDF8C927CF7DA5CA7A7A9FA6375BA3F8FF93928278D5E4A75DE73E77E0FB21840A5DEC384DFBAFE03303F4E5203116C92542AD5849293DA0E9A100A5FFB221F779FF2E4B73281481CFAFF8200BDBF0C2470A236D1CCD3B170524ECEB471208AE8EC02A9334DD25228D932E7BFFE142CEBD40296B248780" };
        }

        public async Task<BaseResponse> UserLoginJwtAsync(LoginViewModel req)
        {
            var UserInfo = await _user.FindWithGroup(a => a.Account == req.Account);
            if (UserInfo == null)//用户名不存在
            {
                return new BaseResponse { Success = false, Message = "输入的用户账号不存在" };
            }
            string p = EncryptData.EncryptPassword(req.Password, UserInfo.Salt);
            if (p != UserInfo.Password)       //密码不正确
            {
                return new BaseResponse { Success = false, Message = "密码不正确" };
            }
            if (UserInfo.Status == UserStatus.InActive)
            {
                return new BaseResponse { Success = false, Message = "该账号未激活，请联系管理员激活" };
            }
            if (UserInfo.Status == UserStatus.InValid)
            {
                return new BaseResponse { Success = false, Message = "该账号为无效账号" };
            }

            if (UserInfo.GroupId == null)     //用户没有加入组织
            {
                return new BaseResponse { Success = false, Message = "用户没有加入组织，不能登录" };
            }
            var rm = _mapper.Map<UserLoginDto>(UserInfo);
            //获取用户的角色
            var r = await _userRole.FindWithRole(UserInfo.Id);
            if (r != null)
            {
                var roles = r.Select(a => a.RoleId).ToList();
                rm.Roles = String.Join(',', roles);
                foreach (var item in r)
                {
                    if (item.Role.IsAdmin)
                    {
                        rm.IsAdmin = true;
                        break;
                    }
                }
            }
            else
            {
                return new BaseResponse { Success = false, Message = "用户没有分配角色不能登录系统，请联系管理员" };
            }
            rm.Success = true;
            rm.Message = "用户登录成功";
            return rm;
        }
        public async Task<BaseResponse> UserRegisterAsync(UserRegisterViewModel req)
        {
            BaseResponse rm = new BaseResponse();
            try
            {
                var um = _mapper.Map<UserModel>(req);
                um.Salt = EncryptData.CreateRandom();
                um.Password = EncryptData.EncryptPassword(req.Password, um.Salt);
                um.Status = UserStatus.InActive;
                um.Create = req.Account;
                await _user.AddAsync(um);
                rm.Success = true;
                rm.Message = "用户注册成功，请等待管理员审核";
            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Message = "注册用户失败，请联系管理员";
                _log.LogError($"{req.Account}注册失败，失败原因{ex.InnerException + ex.Message}");
            }
            return rm;
        }

        public HandleResponse<int> UpdateUser(UserUpdateViewModel req)
        {
            HandleResponse<int> rm = new HandleResponse<int>();

            return rm;
        }

        /// <summary>
        /// 上传头像文件
        /// </summary>
        /// <param name="Id">用户编号</param>
        /// <param name="account">用户账号</param>
        /// <param name="path">文件保存路径</param>
        /// <param name="filePath">图像url</param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateUserImageAsync(int Id, string account, string filePath)
        {
            BaseResponse rm = new BaseResponse();
            var g = _user.Find(Id);
            if (g == null)
            {
                rm.Success = false;
                rm.Message = "用户不存在";
                return rm;
            }
            try
            {
                g.Picture = filePath;
                g.Modify = account;
                g.ModifyTime = DateTime.Now;
                await _user.SaveAsync(g);
                rm.Success = true;
                rm.Message = "上传用户头像成功";
                _log.LogInformation($"{account}上传用户头像成功");
            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Message = "修改数据失败" + ex.Message;
                _log.LogError($"{account}上传头像失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
            }
            return rm;
        }

        public async Task<BaseResponse> UpdateUserInfoAsync(string account, UserUpdateViewModel req, int Id)
        {
            BaseResponse rm = new BaseResponse();
            //var user = await Task.Run(() =>
            //{
            //    return _user.Find(Id);
            //});
            var user = await _user.FindAsync(Id);

            try
            {
                user.UserName = req.UserName;
                user.Phone = req.Phone;
                user.Email = req.Email;
                await _user.SaveAsync(user);
                _log.LogInformation($"{account}修改用户{Id}的信息成功");
                return new BaseResponse { Success = true, Message = "修改用户信息成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改用户信息失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改用户信息失败，请联系管理员" };
            }
        }

        public async Task<bool> IsExist(Expression<Func<UserModel, bool>> predicate)
        {
            var ret = await _user.Find(predicate).FirstOrDefaultAsync();
            if (ret != null)
            {
                return true;
            }
            return false;
        }

        public async Task<string> GetUserGroupAsync(int Id)
        {
            var userInfio = await Task.Run(() =>
            {
                return _user.Find(Id);
            });
            if (userInfio == null)
            {
                return null;
            }
            else
            {
                return userInfio.GroupId;
            }
        }

        public async Task<BaseResponse> UpdateUserStatusAsync(UserStatusUpDateViewModel req, string account)
        {
            var userInfo = _user.Find(req.Id);
            if (userInfo == null)
            {
                return new BaseResponse { Success = false, Message = "该用户不存在" };
            }
            try
            {
                userInfo.Status = (UserStatus)req.Status;
                userInfo.Modify = account;
                userInfo.ModifyTime = DateTime.Now;
                await _user.SaveAsync(userInfo);
                _log.LogInformation($"{account}修改用户{req.Id}的状态为{req.Status}");
                return new BaseResponse { Success = true, Message = "修改用户状态成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{ account}修改id为{req.Id}的状态{req.Status}失败，失败原因：" + ex.StackTrace + "->"
                    + ex.InnerException + "->" + ex.Message);
                return new BaseResponse { Success = false, Message = "修改用户状态失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteUserAsync(int Id, string account)
        {
            var userInfo = _user.Find(Id);
            if (userInfo == null)
            {
                return new BaseResponse { Success = false, Message = "该用户不存在" };
            }
            if (userInfo.Status == UserStatus.Valid)
            {
                return new BaseResponse { Success = false, Message = "只能删除未激活或着无效用户" };
            }
            try
            {
                await _user.RemoveAsync(userInfo);
                _log.LogInformation($"{account}删除用户Id为{Id}用户账号为:{userInfo.Account}成功");
                return new BaseResponse { Success = true, Message = "删除用户成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除用户Id为{Id}用户账号为:{userInfo.Account}失败," + "失败原因:" + ex.Message + "->" +
                   ex.StackTrace + "->" + ex.InnerException);
                return new BaseResponse { Success = false, Message = "删除用户失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateUserPasswordAsync(UserPasswordViewModel req, int Id)
        {
            var userInfo = _user.Find(Id);
            string Password = EncryptData.EncryptPassword(req.OldPassword, userInfo.Salt);
            if (Password != userInfo.Password)
            {
                return new BaseResponse { Success = false, Message = "旧密码不正确" };
            }
            Password = EncryptData.EncryptPassword(req.Password, userInfo.Salt);
            userInfo.Password = Password;
            userInfo.Modify = userInfo.Account;
            userInfo.ModifyTime = DateTime.Now;
            try
            {
                await _user.SaveAsync(userInfo);
                _log.LogInformation("用户修改密码成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"用户修改密码失败:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "用户修改密码失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> ResetPassword(UserResetPasswordViewModel req, string account)
        {
            var userInfo = _user.Find(req.Id);
            if (userInfo == null)
            {
                return new BaseResponse { Success = false, Message = "输入用户编号不存在" };
            }
            try
            {
                string password = EncryptData.EncryptPassword(req.Password, userInfo.Salt);
                userInfo.Password = password;
                userInfo.Modify = account;
                userInfo.ModifyTime = DateTime.Now;
                await _user.SaveAsync(userInfo);
                _log.LogInformation($"{account}修改{userInfo.Id}->{userInfo.Account}密码成功");
                return new BaseResponse { Success = true, Message = "重置密码成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改{userInfo.Id}->{userInfo.Account}密码失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "重置密码失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> AddUserAsync(string Account, string GroupId, UserAddViewModel req)
        {
            var user = await _user.Find(a => a.Account == req.Account).FirstOrDefaultAsync();
            if (user != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同的账号" };
            }
            try
            {
                var um = _mapper.Map<UserModel>(req);
                um.Salt = EncryptData.CreateRandom();
                um.Password = EncryptData.EncryptPassword(req.Password, um.Salt);
                um.Status = UserStatus.Valid;
                um.Create = Account;
                um.GroupId = GroupId;
                await _user.AddAsync(um);
                _log.LogInformation($"添加标示为{um.Id}的用户成功");
                return new HandleResponse<int> { Success = true, Message = "添加用户成功", Key = um.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"添加用户失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加用户失败，请联系管理员" };
            }
        }

    }
}
