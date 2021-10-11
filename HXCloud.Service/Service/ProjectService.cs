using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
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
        private readonly IDeviceRepository _dr;
        private readonly IRoleProjectRepository _rp;//获取用户分配的项目
        private readonly IRegionRepository _rr;

        public ProjectService(IProjectRepository pr, ILogger<ProjectService> log, IMapper mapper, IDeviceRepository dr, IRoleProjectRepository rp, IRegionRepository rr)
        {
            this._pr = pr;
            this._log = log;
            this._mapper = mapper;
            this._dr = dr;
            this._rp = rp;
            this._rr = rr;
        }
        public async Task<bool> CheckProjectIdIsTopProjectAsync(int projectId)
        {
            var data = await _pr.FindAsync(projectId);
            if (data==null)//判断是否存在
            {
                return false;
            }
            if (data.ParentId==null)//是否存在父项目
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 如果返回0值表示该项目不存在
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<int> GetTopProjectIdAsync(int projectId)
        {
            var data = await _pr.FindAsync(projectId);
            if (data==null)
            {
                return 0;
            }
            if (data.ParentId==null)
            {
                return projectId;
            }
            else
            {
              return  await GetTopProjectIdAsync(data.ParentId.Value);
            }
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
        public async Task<ProjectCheckDto> GetProjectCheckAsync(int Id)
        {
            var p = new ProjectCheckDto();
            var data = await _pr.FindAsync(Id);
            if (data == null)
            {
                p.IsExist = false;
            }
            else
            {
                p.IsExist = true;
                p.GroupId = data.GroupId;
                if (data.PathId == null | "" == data.PathId)
                {
                    p.IsSite = false;
                    p.PathId = Id.ToString();
                }
                else
                {
                    p.IsSite = data.ProjectType == ProjectType.Site ? true : false;
                    p.PathId = data.PathId;
                }
            }
            return p;
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
            var data = await _pr.Find(a => a.GroupId == GroupId && a.ParentId == req.ParentId && a.Name == req.Name).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该项目下已存在相同名称的项目或者场站" };
            }

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
          
            try
            {
                var entity = _mapper.Map<ProjectModel>(req);
                entity.Create = account;
                entity.PathId = pathId;
                entity.PathName = PathName;
                entity.GroupId = GroupId;
                await _pr.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}项目名称为{entity.Name}的项目成功");
                return new HandleResponse<int> { Success = true, Message = "添加成功", Key = entity.Id };
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

        //只获取项目,包含项目下设备数据
        public async Task<BaseResponse> GetProjectByIdAsync(int Id)
        {
            var data = await _pr.FindWithImageAndChildAsync(a => a.Id == Id).FirstOrDefaultAsync();
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的项目或者场站不存在" };
            }
            var dto = _mapper.Map<ProjectDto>(data);
            //获取项目下所有的场站
            var sites = await GetProjectSitesIdAsync(Id);
            //获取项目下设备的数量
            var count = await _dr.Find(a => sites.Contains(a.ProjectId.Value)).CountAsync();
            dto.DeviceCount = count;
            //获取项目的区域名称
            if (data.RegionId != null && "" != data.RegionId)
            {
                var r = await _rr.FindAsync(data.RegionId, data.GroupId);
                if (r != null)
                {
                    dto.RegionName = r.Name;
                }
            }
            return new BResponse<ProjectDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        //根据项目标示获取项目下的子项目(不含嵌套数据,不包含项目下设备的数量)
        public async Task<BaseResponse> GetChildProjectByIdAsync(int Id, ProjectPageRequest req)
        {
            var data = _pr.FindWithImageAndChildAsync(a => a.ParentId == Id);
            if (req.ProjectType == 1)  //项目
            {
                data = data.Where(a => a.ProjectType == ProjectType.Project);
            }
            else if (req.ProjectType == 2)//场站
            {
                data = data.Where(a => a.ProjectType == ProjectType.Site);
            }
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => a.Name.Contains(req.Search));
            }
            int count = data.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var list = await data.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<ProjectDto>>(list);
            //获取项目的区域名称
            foreach (var item in dtos)
            {
                //获取项目或者场站下设备的数量
                if (item.ProjectType == (int)ProjectType.Project)
                {
                    var sites = await GetProjectSitesIdAsync(item.Id);
                    int num = await _dr.Find(a => sites.Contains(a.ProjectId.Value)).CountAsync();
                    item.DeviceCount = num;
                }
                else
                {
                    int num = await _dr.Find(a => a.ProjectId == item.Id).CountAsync();
                    item.DeviceCount = num;
                }
                //获取区域名称
                if (item.RegionId != null && "" != item.RegionId)
                {
                    var r = await _rr.FindAsync(item.RegionId, item.GroupId);
                    if (r != null)
                    {
                        item.RegionName = r.Name;
                    }
                }
            }
            var br = new BasePageResponse<List<ProjectDto>>();
            br.Success = true;
            br.Message = "获取数据成功";
            br.PageSize = req.PageSize;
            br.CurrentPage = req.PageNo;
            br.Count = count;
            br.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            br.Data = dtos;
            return br;
        }

        //获取单个项目（会递归获取项目下的所有项目或者场站）
        public async Task<BaseResponse> GetProjectWithChildAsync(int Id)
        {
            var data = await _pr.FindProjectsWithImageByParentAsync(a => a.Id == Id).ToListAsync();
            //if (data == null)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的项目或者场站不存在" };
            //}
            var dto = _mapper.Map<List<ProjectDto>>(data);
            //if (data.Images.Count > 0)
            //{
            //    dto.Image = data.Images.OrderByDescending(a => a.Rank).FirstOrDefault().url;
            //}
            //dto.Child = new List<ProjectData>();
            //if (data.Child.Count > 0)
            //{
            //    await GetChild(dto, Id);
            //}
            foreach (var item in dto)
            {
                var sites = await GetProjectSitesIdAsync(item.Id);
                int num = await _dr.Find(a => sites.Contains(a.ProjectId.Value)).CountAsync();
                item.DeviceCount = num;
            }
            return new BResponse<List<ProjectDto>> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task GetChild(ProjectData td, int Id)
        {
            var ret = await _pr.FindWithImageAndChildAsync(a => a.ParentId == Id).ToListAsync();
            foreach (var item in ret)
            {
                var dto = _mapper.Map<ProjectData>(item);
                //if (item.Images.Count > 0)
                //{
                //    dto.Image = item.Images.OrderByDescending(a => a.Rank).FirstOrDefault().url;
                //}
                td.Child.Add(dto);
                await GetChild(dto, item.Id);
            }
        }

        //获取组织下的所有项目或者场站
        public async Task<BaseResponse> GetGroupProject(string GroupId)
        {
            List<ProjectData> list = new List<ProjectData>();
            //获取该组织下所有的顶级项目
            var datas = await _pr.FindWithImageAndChildAsync(a => a.Parent == null && a.GroupId == GroupId).ToListAsync();
            foreach (var item in datas)
            {
                var dto = _mapper.Map<ProjectData>(item);
                //if (item.Images.Count > 0)
                //{
                //    dto.Image = item.Images.OrderByDescending(a => a.Rank).FirstOrDefault().url;
                //}
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

        public async Task<BaseResponse> DeleteProjectAsync(string account, int Id)
        {
            var entity = await _pr.FindAsync(Id);
            if (entity == null)
            {
                return new BaseResponse { Success = false, Message = "输入的项目或者场站不存在" };
            }
            if (entity.ProjectType == ProjectType.Project)
            {
                //是否有子项目或者有设备挂载
                var child = await _pr.Find(a => a.ParentId == Id).CountAsync();
                if (child > 0)
                {
                    return new BaseResponse { Success = false, Message = "该项目下有子项目或者场站，不能删除" };
                }
            }
            else
            {
                var device = await _dr.Find(a => a.ProjectId == Id).CountAsync();
                if (device > 0)
                {
                    return new BaseResponse { Success = false, Message = "该场站下挂在有设备,不能删除" };
                }
            }
            try
            {
                await _pr.RemoveAsync(entity);
                _log.LogInformation($"{account}删除标识为{Id}的项目或者场站成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标识为{Id}的项目或者场站失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }

        }

        public async Task<BaseResponse> GetMyProjectAsync(string GroupId, string roles, bool isAdmin, BasePageRequest req)
        {
            //获取用户所有的项目标识
            //var pids = await GetMyProjectIdSync(GroupId, roles, isAdmin);
            var pids = await GetMyTopProjectIdAsync(GroupId, roles, isAdmin);
            //var project = _pr.Find(a => a.GroupId == GroupId && pids.Contains(a.Id));
            var project = _pr.FindWithImageAndChildAsync(a => a.GroupId == GroupId && pids.Contains(a.Id));
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                project = project.Where(a => a.Name.Contains(req.Search));
            }
            int count = project.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var list = await project.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dto = _mapper.Map<List<ProjectDto>>(list);
            foreach (var item in dto)
            {
                var sites = await GetProjectSitesIdAsync(item.Id);
                int num = await _dr.Find(a => sites.Contains(a.ProjectId.Value)).CountAsync();
                item.DeviceCount = num;
            }
            //foreach (var item in list)
            //{
            //    var dto = _mapper.Map<ProjectDto>(item);
            //    //if (item.Images.Count > 0)
            //    //{
            //    //    dto.Image = item.Images.OrderByDescending(a => a.Rank).FirstOrDefault().url;
            //    //}
            //    //dto.Image = item.Images.OrderByDescending(a => a.Rank).FirstOrDefault().url;
            //    await GetChild(dto, item.Id);
            //    dtos.Add(dto);
            //}
            //var dto = _mapper.Map<List<ProjectData>>(list);
            var br = new BasePageResponse<List<ProjectDto>>();
            br.Success = true;
            br.Message = "获取数据成功";
            br.PageSize = req.PageSize;
            br.CurrentPage = req.PageNo;
            br.Count = count;
            br.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            br.Data = dto;
            return br;
        }
        public async Task<BaseResponse> GetMySiteAsync(string GroupId, string roles, bool isAdmin, BasePageRequest req)
        {
            //获取用户所有的项目标识
            var sites = await GetMySitesIdAsync(GroupId, roles, isAdmin);
            var project = _pr.FindWithImageAndDeviceAsync(a => a.GroupId == GroupId && sites.Contains(a.Id));
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                project = project.Where(a => a.Name.Contains(req.Search));
            }
            int count = project.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var list = await project.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();

            var dto = _mapper.Map<List<SiteDto>>(list);
            var br = new BasePageResponse<List<SiteDto>>();
            br.Success = true;
            br.Message = "获取数据成功";
            br.PageSize = req.PageSize;
            br.CurrentPage = req.PageNo;
            br.Count = count;
            br.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            br.Data = dto;
            return br;
        }

        //获取我的全部顶级项目
        public async Task<List<int>> GetMyTopProjectIdAsync(string GroupId, string roles, bool isAdmin)
        {
            List<int> pids = new List<int>();
            if (isAdmin)
            {
                pids = await _pr.Find(a => a.GroupId == GroupId && a.Parent == null).Select(a => a.Id).ToListAsync();

            }
            else
            {
                //获取用户分配的项目
                int[] rs = Array.ConvertAll<string, int>(roles.Split(','), src => int.Parse(src));
                //包含有场站
                pids = await _rp.Find(a => rs.Contains(a.RoleId) && (int)a.Operate >= 0).Select(a => a.ProjectId).ToListAsync();
                var s = await _pr.Find(a => pids.Contains(a.Id) && a.ProjectType == ProjectType.Site).Select(a => a.Id).ToArrayAsync();
                //移除场站
                pids.RemoveAll(a => s.Contains(a));
            }
            //var c = await GetChildId(pids);
            //pids.AddRange(c);
            return pids;
        }

        /// <summary>
        /// 获取角色项目标识
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="roles"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public async Task<List<int>> GetMyProjectIdSync(string GroupId, string roles, bool isAdmin)
        {
            List<int> pids = new List<int>();
            if (isAdmin)
            {
                pids = await _pr.Find(a => a.GroupId == GroupId && a.Parent == null).Select(a => a.Id).ToListAsync();

            }
            else
            {
                //获取用户分配的项目
                int[] rs = Array.ConvertAll<string, int>(roles.Split(','), src => int.Parse(src));
                //包含有场站
                pids = await _rp.Find(a => rs.Contains(a.RoleId) && (int)a.Operate >= 0).Select(a => a.ProjectId).ToListAsync();
                var s = await _pr.Find(a => pids.Contains(a.Id) && a.ProjectType == ProjectType.Site).Select(a => a.Id).ToArrayAsync();
                //移除场站
                pids.RemoveAll(a => s.Contains(a));
            }
            var c = await GetChildId(pids);
            pids.AddRange(c);
            return pids;
        }
        //获取项目下的所有场站编号
        public async Task<List<int>> GetProjectSitesIdAsync(int project)
        {
            List<int> p = new List<int> { project };
            var pIds = await GetChildId(p);
            pIds.Add(project);
            var sites = await _pr.Find(a => pIds.Contains(a.ParentId.Value) && a.ProjectType == ProjectType.Site).Select(a => a.Id).ToListAsync();
            return sites;
        }
        public async Task<List<int>> GetChildId(List<int> id)
        {
            var p = await _pr.Find(a => id.Contains(a.ParentId.Value) && a.ProjectType == ProjectType.Project).Select(a => a.Id).ToListAsync();
            if (p.Count > 0)
            {
                var c = await GetChildId(p);
                p.AddRange(c);
            }
            return p;
        }
        public async Task<List<int>> GetMySitesIdAsync(string GroupId, string roles, bool isAdmin)
        {
            var pids = await GetMyProjectIdSync(GroupId, roles, isAdmin);
            
            var sites = await _pr.Find(a => pids.Contains(a.ParentId.Value) && a.ProjectType == ProjectType.Site).Select(a => a.Id).ToListAsync();
            //获取用户直属的场站标识
            if (!isAdmin)//管理员在项目编号中获取所有的场站
            {
                sites.AddRange(await GetRolesSitesAsync(GroupId, roles));
            }
            return sites;
        }
        /// <summary>
        /// 获取普通用户直属的场站标识
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="roles"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public async Task<List<int>> GetRolesSitesAsync(string GroupId, string roles)
        {
            //获取用户分配的项目
            int[] rs = Array.ConvertAll<string, int>(roles.Split(','), src => int.Parse(src));
            //包含有场站
            var pids = await _rp.Find(a => rs.Contains(a.RoleId) && (int)a.Operate >= 0).Select(a => a.ProjectId).ToListAsync();
            var s = await _pr.Find(a => pids.Contains(a.Id) && a.ProjectType == ProjectType.Site).Select(a => a.Id).ToListAsync();
            return s;
        }
    }
}
