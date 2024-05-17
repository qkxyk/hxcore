using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HXCloud.Service
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userrole;
        private readonly ILogger<UserRoleService> _log;
        private readonly IMapper _mapper;

        public UserRoleService(IUserRoleRepository userrole, ILogger<UserRoleService> log, IMapper mapper)
        {
            _userrole = userrole;
            this._log = log;
            this._mapper = mapper;
        }

        public async Task<BaseResponse> AddUserRoleAsync(UserRoleAddViewModel req, string account)
        {
            BaseResponse rm = new BaseResponse();
            try
            {
                await _userrole.SaveAsync(req.UserId, req.RoleId, account);
                rm.Success = true;
                rm.Message = "用户分配角色成功";
                _log.LogInformation($"{account}分配用户标示{req.UserId}角色{req.RoleId.ToString()}成功");
            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Message = "分配角色失败";
                _log.LogError($"{account}分配用户标示{req.UserId}角色{req.RoleId.ToString()}失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
            }
            return rm;
        }
        public async Task<BaseResponse> GetUserRole(int UserId)
        {
            var ur = await _userrole.FindWithRole(UserId);
            var urDto = _mapper.Map<List<UserRoleData>>(ur);
            BaseResponse br = new BResponse<List<UserRoleData>> { Success = true, Message = "获取数据成功", Data = urDto };
            return br;
        }

        /// <summary>
        /// 根据用户标识获取用户角色(角色用逗号分割)
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <returns></returns>
        public async Task<string> GetUserRolesAsync(int userId)
        {
            var ur = await _userrole.Find(a=>a.UserId==userId).Select(a=>a.RoleId).ToListAsync();
            var ret = String.Join(',', ur);
            return ret;
        }
        [Obsolete]
        public HandleResponse<UserRoleKey> DeleteUserRole(UserRoleDeleteViewModel req)
        {
            HandleResponse<UserRoleKey> rm = new HandleResponse<UserRoleKey>();
            //var b = IsExist(a => a.UserId == req.UserId && a.RoleId == req.RoleId);
            //if (b)
            //{
            //    rm.Success = false;
            //    rm.Message = "用户没有分配该角色，请确认";
            //    return rm;
            //}
            UserRoleModel ur = new UserRoleModel() { UserId = req.UserId, RoleId = req.RoleId };
            try
            {
                _userrole.Remove(ur);
                rm.Success = true;
                rm.Message = "删除数据成功";
                rm.Key = new UserRoleKey { UserId = req.UserId, RoleId = req.RoleId };
            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Message = "删除数据失败" + ex.Message;
            }
            return rm;
        }
        public async Task<bool> IsExist(Expression<Func<UserRoleModel, bool>> predicate)
        {
            var ret = await _userrole.Find(predicate).FirstOrDefaultAsync();
            if (ret == null)
            {
                return false;
            }
            return true;
        }
    }
}
