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
    public class RoleProjectService : IRoleProjectService
    {
        private readonly ILogger<RoleProjectService> _log;
        private readonly IRoleProjectRepository _rp;
        private readonly IMapper _mapper;

        public RoleProjectService(ILogger<RoleProjectService> log, IRoleProjectRepository rp, IMapper mapper)
        {
            this._log = log;
            this._rp = rp;
            this._mapper = mapper;
        }
        public Task<bool> IsExist(Expression<Func<RoleProjectModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        //验证角色是否权限(非管理员)
        public async Task<bool> IsAuth(string roles, string pathId, int operate)
        {
            if (pathId.Trim().Count() == 0)  //无项目的设备只有管理员有权限
            {
                return false;
            }
            int[] rs = Array.ConvertAll<string, int>(roles.Split(','), src => int.Parse(src));
            var data = await _rp.Find(a => rs.Contains(a.RoleId) && (int)a.Operate >= operate).ToListAsync();
            if (data == null)
            {
                return false;
            }
            int[] ps = Array.ConvertAll(pathId.Split('/'), src => int.Parse(src));
            foreach (var item in data)
            {
                if (ps.Contains(item.ProjectId))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取角色项目
        /// </summary>
        /// <param name="roleId">金额us标示</param>
        /// <returns>角色项目列表</returns>
        public async Task<BaseResponse> GetRoleProjectAsync(int roleId)
        {
            var data = await _rp.Find(a => a.RoleId == roleId).ToListAsync();
            var dtos = _mapper.Map<List<RoleProjectDto>>(data);
            return new BResponse<List<RoleProjectDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }

        /// <summary>
        /// 更改角色项目全新啊
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="RoleId">角色标示</param>
        /// <param name="projects">项目或者场站列表</param>
        /// <param name="operate">操作</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddOrUpdateRoleProjectAsync(string Account, int RoleId, int[] projects, int[] operate)
        {
            //验证输入的项目编号是否存在
            List<RoleProjectModel> list = new List<RoleProjectModel>();
            for (int i = 0; i < projects.Length; i++)
            {
                list.Add(new RoleProjectModel { Create = Account, RoleId = RoleId, ProjectId = projects[i], Operate = (ProjectOperate)operate[i] });
            }
            try
            {
                await _rp.SaveAsync(RoleId, list);
                _log.LogInformation($"{Account}修改角色{RoleId}的项目权限成功");
                return new BaseResponse { Success = true, Message = "更改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改角色{RoleId}的项目权限失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "更改数据失败，请联系管理员" };
            }
        }
    }
}
