using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HXCloud.Service
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _log;

        private IRoleRepository _role { get; }
        public RoleService(IRoleRepository role, IMapper mapper, ILogger<RoleService> log)
        {
            _role = role;
            this._mapper = mapper;
            this._log = log;
        }
        public async Task<BaseResponse> AddRoleAsync(RoleAddDto req, string account, string GroupId)
        {
            BaseResponse rm = new BaseResponse();
            //检查该组织下该部门下是否存在相同的角色
            var r = await IsExist(a => a.RoleName == req.Name /*&& a.GroupId == req.gr*/);
            if (r)
            {
                rm.Success = false;
                rm.Message = "该部门下存在相同名称的角色，请确认";
                return rm;
            }
            try
            {
                var role = _mapper.Map<RoleModel>(req);
                role.Create = account;
                role.GroupId = GroupId;
                await _role.AddAsync(role);
                rm = new HandleResponse<int> { Key = role.Id, Success = true, Message = "添加角色成功" };
                _log.LogInformation($"{account}创建角色{req.Name},角色编号为{role.Id}成功");
            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Message = "添加角色失败";
                _log.LogError($"{account}创建角色{req.Name}失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
            }
            return rm;
        }

        public async Task<BaseResponse> GetRole(int RoleId)
        {
            BaseResponse br = new BaseResponse();
            br = await Task.Run(() =>
              {
                  var ret = _role.Find(RoleId);
                  if (ret == null)
                  {
                      br.Success = false;
                      br.Message = "输入的角色编号不存在";
                      return br;
                  }
                  var d = _mapper.Map<RoleDataDto>(ret);
                  br = new BResponse<RoleDataDto>() { Success = true, Message = "获取数据成功", Data = d };
                  return br;
              });
            return br;
        }

        public async Task<BaseResponse> GetRoles(string GroupId, BasePageRequest req)
        {
            BasePageResponse<List<RoleDataDto>> br = new BasePageResponse<List<RoleDataDto>>();
            var g = _role.Find(a => a.GroupId == GroupId);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                g = g.Where(a => a.RoleName.Contains(req.Search) || a.Description.Contains(req.Search));
            }
            int count = g.Count();
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
            var list = await g.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dto = _mapper.Map<List<RoleDataDto>>(list);
            br.Success = true;
            br.Message = "获取数据成功";
            br.PageSize = req.PageSize;
            br.CurrentPage = req.PageNo;
            br.Count = count;
            br.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            br.Data = dto;
            return br;
        }

        public async Task<BaseResponse> DeleteRoleAsync(int roleId, string account)
        {
            var role = _role.Find(roleId);
            if (role == null)
            {
                return new BaseResponse { Success = false, Message = "输入的角色不存在" };
            }
            try
            {
                await _role.RemoveAsync(role);
                _log.LogInformation($"{account}删除角色{role.Id}->{role.RoleName}成功");
                return new BaseResponse { Success = true, Message = "删除角色信息成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除角色{role.Id}:{role.RoleName}失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除角色失败，请联系管理员" };
            }
        }

        public async Task<bool> IsExist(Expression<Func<RoleModel, bool>> predicate)
        {
            var ret = await _role.Find(predicate).FirstOrDefaultAsync();
            if (ret != null)
            {
                return true;
            }
            return false;
        }

        public async Task<BaseResponse> UpdateRoleAsync(RoleUpdateDto req, string account, string GroupId)
        {
            var role = await _role.Find(a => a.Id == req.Id && a.GroupId == GroupId).FirstOrDefaultAsync();
            if (role == null)
            {
                return new BaseResponse { Success = false, Message = "输入的角色不存在" };
            }
            try
            {
                var r = _mapper.Map(req, role);
                await _role.SaveAsync(r);
                _log.LogInformation($"{account}修改{req.Id}的角色信息成功");
                return new BaseResponse { Success = true, Message = "修改角色信息成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改角色失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改角色信息失败，请联系管理员" };
            }
        }

        public async Task<List<int>> GetRoles(Expression<Func<RoleModel, bool>> predicate)
        {
            var roles = await _role.Find(predicate).Select(a => a.Id).ToListAsync();
            return roles;
        }
    }
}
