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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _pr;
        private readonly ILogger<ProjectService> _log;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository pr, ILogger<ProjectService> log, IMapper mapper)
        {
            this._pr = pr;
            this._log = log;
            this._mapper = mapper;
        }

        //检查输入的项目或者场站是否存在，并返回项目或者场站的路径以及所属的组织编号
        public bool IsExist(int Id, out string pathId, out string groupId)
        {
            var data = _pr.Find(Id);
            if (data == null)
            {
                pathId = null;
                groupId = null;
                return false;
            }
            else
            {
                pathId = data.PathId;
                groupId = data.GroupId;
                return true;
            }
        }
        public async Task<string> GetPathId(int Id)
        {
            var data = await _pr.FindAsync(Id);
            if (data == null)
            {
                return null;
            }
            if (data.PathId == null | "" == data.PathId)
            {
                return Id.ToString();
            }
            else
            {
                return data.PathId;
            }
        }

        public async Task<string> GetGroupIdAsync(int projectId)
        {
            var ret = await _pr.FindAsync(projectId);
            if (ret == null)
            {
                return null;
            }
            return ret.GroupId;
        }
        public async Task<bool> IsExist(Expression<Func<ProjectModel, bool>> predicate)
        {
            var ret = await _pr.Find(predicate).FirstOrDefaultAsync();
            if (ret == null)
            {
                return false;
            }
            return true;
        }
        public async Task<BaseResponse> AddProjectAsync(ProjectAddDto req, string account, string GroupId)
        {
            string pathId = null, PathName = null;
            //获取父项目
            if (req.ParentId.HasValue)     //存在父节点
            {
                var parent = await _pr.FindAsync(req.ParentId.Value);
                if (parent == null)
                {
                    return new BaseResponse { Success = false, Message = "输入的父项目不存在" };
                }
                if (parent.ParentId == null)
                {
                    pathId = $"{req.ParentId.Value}";
                    PathName = $"{parent.Name}";
                }
                else
                {
                    pathId = $"{parent.PathId}/{req.ParentId.Value}";
                    PathName = $"{parent.PathName}/{parent.Name}";
                }
            }
            var data = await _pr.Find(a => a.GroupId == GroupId && a.ParentId == req.ParentId && a.Name == req.Name).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该项目下已存在相同名称的项目或者场站" };
            }
            try
            {
                var entity = _mapper.Map<ProjectModel>(req);
                entity.Create = account;
                entity.PathId = pathId;
                entity.PathName = PathName;
                entity.GroupId = GroupId;
                await _pr.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}项目名称为{entity.Name}的项目成功");
                return new BResponse<int> { Success = true, Message = "添加成功", Data = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加项目{req.Name}失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加失败，请联系管理员" };
            }
        }

        //只支持变更名称、位置、区域和地域
        public async Task<BaseResponse> UpdateProjectAsync(ProjectUpdateDto req, string account)
        {
            var data = await _pr.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的项目或者场站不存在" };
            }
            //同一个项目下不能重名
            var ret = await _pr.Find(a => a.ParentId == data.ParentId && a.Name == req.Name && a.Id != req.Id).FirstOrDefaultAsync();
            if (ret != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的项目获取场站" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _pr.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的项目或者场站信息成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的项目或者场站失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }

        //获取单个项目（会递归获取项目下的所有项目或者场站）
        public async Task<BaseResponse> GetProject(int Id)
        {
            var data = await _pr.FindAsync(a => a.Id == Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的项目或者场站不存在" };
            }
            var dto = _mapper.Map<ProjectData>(data);
            //dto.Child = new List<ProjectData>();
            if (data.Child.Count > 0)
            {
                await GetChild(dto, Id);
            }
            return new BResponse<ProjectData> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task GetChild(ProjectData td, int Id)
        {
            var ret = await _pr.Find(a => a.ParentId == Id).ToListAsync();
            foreach (var item in ret)
            {
                var dto = _mapper.Map<ProjectData>(item);
                td.Child.Add(dto);
                await GetChild(dto, item.Id);
            }
        }

        //获取组织下的所有项目或者场站
        public async Task<BaseResponse> GetGroupProject(string GroupId)
        {
            List<ProjectData> list = new List<ProjectData>();
            //获取该组织下所有的顶级项目
            var datas = await _pr.Find(a => a.Parent == null && a.GroupId == GroupId).ToListAsync();
            foreach (var item in datas)
            {
                var dto = _mapper.Map<ProjectData>(item);
                await GetChild(dto, item.Id);
                list.Add(dto);
            }
            return new BResponse<List<ProjectData>> { Success = true, Message = "获取数据成功", Data = list };
        }

        public async Task<ProjectModel> GetProjectAsync(int Id)
        {
            var ret = await _pr.FindAsync(Id);
            if (ret == null)
            {
                return null;
            }
            return ret;
        }

    }
}
