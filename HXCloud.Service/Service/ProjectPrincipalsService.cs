using System;
using System.Collections.Generic;
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
    public class ProjectPrincipalsService : IProjectPrincipalsService
    {
        private readonly ILogger<ProjectPrincipalsService> _log;
        private readonly IMapper _mapper;
        private readonly IProjectPrincipalsRepository _pp;

        public ProjectPrincipalsService(ILogger<ProjectPrincipalsService> log, IMapper mapper, IProjectPrincipalsRepository pp)
        {
            this._log = log;
            this._mapper = mapper;
            this._pp = pp;
        }
        public Task<bool> IsExist(Expression<Func<ProjectPrincipalsModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> AddProjectPrincipalsAsync(string account, int projectId, ProjectPrincipalsAddDto req)
        {
            var count = await _pp.Find(a => a.Name == req.Name && a.ProjectId == projectId).CountAsync();
            if (count>0)
            {
                return new BaseResponse { Success = false, Message = $"该项目下已添加过{req.Name}" };
            }
            try
            {
                var entity = _mapper.Map<ProjectPrincipalsModel>(req);
                entity.Create = account;
                entity.ProjectId = projectId;
                await _pp.AddAsync(entity);
                _log.LogInformation($"{account}添加项目运维人员成功，标示为{entity.Id}");
                return new HandleResponse<int> { Success = true, Message = "添加运维人员成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加项目运维人员失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加添加项目运维人员失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateProjectPrincipalsAsync(string account, int Id, ProjectPrincipalsUpdateDto req)
        {
            var d = await _pp.FindAsync(Id);
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维人员不存在" };
            }
            var count = await _pp.Find(a => a.Name == req.Name && a.Id != Id).CountAsync();
            if (count>0)
            {
                return new BaseResponse { Success = false, Message = $"该项目下已存在名字为{req.Name}的运维人员" };
            }
            try
            {
                var entity = _mapper.Map(req, d);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _pp.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{Id}的项目运维人员信息成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{Id}的项目运维人员信息失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> RemoveProjectPrincipalsAsync(int Id, string account)
        {
            var ret = await _pp.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据不存在" };
            }
            try
            {
                              await _pp.RemoveAsync(ret);
                _log.LogInformation($"{account}删除Id为｛Id｝项目运维人员信息成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除Id为{Id}的项目运维人员信息失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> GetPrincipalAsync(int Id)
        {
            var data = await _pp.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据不存在" };
            }
            var dto = _mapper.Map<ProjectPrincipalsDto>(data);
            return new BResponse<ProjectPrincipalsDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetProjectPrincipalsAsync(int pId)
        {
            var data = await _pp.Find(a => a.ProjectId == pId).ToListAsync();
            var dtos = _mapper.Map<List<ProjectPrincipalsDto>>(data);
            return new BResponse<List<ProjectPrincipalsDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
    }
}
