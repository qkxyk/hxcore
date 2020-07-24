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

namespace HXCloud.Service.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ILogger<DepartmentService> _log;
        private readonly IMapper _map;

        private IDepartmentRepository _department { get; }
        public DepartmentService(IDepartmentRepository department, ILogger<DepartmentService> log, IMapper map)
        {
            _department = department;
            this._log = log;
            this._map = map;
        }

        /// <summary>
        /// 添加部门，已验证是否重名，调用端只需验证权限
        /// </summary>
        /// <param name="req">部门信息</param>
        /// <param name="groupId">组织编号</param>
        /// <returns>返回添加部门是否成功</returns>
        public async Task<BaseResponse> AddDepartmentAsync(DepartmentAddViewModel req, string account)
        {
            int level = 1;
            string pathId = null, pathName = null;
            BaseResponse rm = new BaseResponse();
            //查看是否存在多个顶级部门
            if (req.ParentId == null || req.ParentId.Value == 0)
            {
                if (req.DepartmentType == 1)
                {
                    return new BaseResponse { Success = false, Message = "不能直接添加岗位" };
                }
                var dep = await _department.FindAsync(a => a.GroupId == req.GroupId && (a.ParentId.Value == 0 || a.ParentId == null));
                if (dep != null)
                {
                    return new BaseResponse { Success = false, Message = "一个组织只能添加一个顶级部门" };
                }
            }
            else
            {
                var parent = await _department.FindAsync(req.ParentId.Value);
                if (parent == null)
                {
                    return new BaseResponse { Success = false, Message = "输入的父部门不存在" };
                }
                level = parent.Level + 1;
                if (parent.ParentId == null)
                {
                    pathId = parent.Id.ToString();
                    pathName = parent.DepartmentName;
                }
                else
                {
                    pathId = $"{ parent.PathId}/{parent.Id}";
                    pathName = $"{parent.PathName}/{parent.DepartmentName}";
                }
                if (parent.DepartmentType == DepartmentType.Station)
                {
                    return new BaseResponse { Success = false, Message = "岗位下不能再添加部门或者岗位" };
                }
            }

            //同一部门下的子部门不能重名
            var d = _department.Find(a => a.ParentId == req.ParentId && a.DepartmentName == req.Name && a.GroupId == req.GroupId).FirstOrDefault();
            if (d != null)
            {
                rm.Success = false;
                rm.Message = "该组织下存在相同的部门名称";
                return rm;
            }
            try
            {
                var data = _map.Map<DepartmentModel>(req);
                data.Create = account;
                data.GroupId = req.GroupId;
                data.Level = level;
                data.PathId = pathId;
                data.PathName = pathName;
                await _department.AddAsync(data);
                rm = new BResponse<int>()
                {
                    Success = true,
                    Message = "添加数据成功",
                    Data = data.Id
                };
                _log.LogInformation($"{account}添加id为{data.Id},名称为:{req.Name}的部门成功");
            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Message = "添加数据失败";
                _log.LogError($"{account}添加名称为{req.Name}的部门失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
            }
            return rm;
        }
        /// <summary>
        /// 更新部门名称，已验证部门是否存在和是否存在相同的名称，调用端只需要验证是否有权限操作
        /// </summary>
        /// <param name="req">部门名称等信息</param>
        /// <param name="groupId">组织标识</param>
        /// <returns>返回更新部门名称是否成功</returns>
        public async Task<BaseResponse> UpdateDepartmentAsync(DepartmentUpdateViewModel req, string groupId, string account)
        {
            BaseResponse rm = new BaseResponse();
            var dm = await _department.FindAsync(req.Id);
            if (dm == null)
            {
                rm.Success = false;
                rm.Message = "该部门不存在";
                return rm;
            }
            //同一部门下的子部门不能重名
            var d = _department.Find(a => a.ParentId == dm.ParentId && a.DepartmentName == req.Name && a.GroupId == groupId).FirstOrDefault();
            if (d != null && d.Id != req.Id)
            {
                rm.Success = false;
                rm.Message = "该组织下存在相同的部门名称";
                return rm;
            }
            try
            {
                //dm = _map.Map<DepartmentModel>(req);
                _map.Map(req, dm);
                dm.Modify = account;
                dm.ModifyTime = DateTime.Now;
                //dm.DepartmentName = req.Name;
                await _department.SaveAsync(dm);
                rm.Success = true;
                rm.Message = "修改数据成功";
                _log.LogInformation($"{account}修改Id为{req.Id}的部门名称为{req.Name}成功");
            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Message = "修改数据失败";
                _log.LogError($"{account}修改id为{req.Id}的部门名称失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
            }
            return rm;
        }

        /// <summary>
        /// 删除部门，需要验证是否有子部门，该部门下是否还有角色和用户
        /// </summary>
        /// <param name="id">部门标识</param>
        /// <returns>删除部门是否成功</returns>
        public async Task<BaseResponse> DeleteDepartmentAsync(int id, string account)
        {
            BaseResponse rm = new BaseResponse();
            var dm = await _department.FindAsync(a => a.Id == id);
            if (dm == null)
            {
                rm.Success = false;
                rm.Message = "此部门不存在";
                return rm;
            }
            else if (dm.Child.Count != 0)
            {
                rm.Success = false;
                rm.Message = "此部门还存在子部门，不能删除";
                return rm;
            }
            try
            {
                await _department.RemoveAsync(dm);
                rm.Success = true;
                rm.Message = "删除数据成功";
                _log.LogInformation($"{account}删除Id为{id},名称为{dm.DepartmentName}的部门成功");
            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Message = "删除数据失败";
                _log.LogError($"{account}删除Id为{id},名称为{dm.DepartmentName}的部门失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
            }
            return rm;
        }

        public async Task<BaseResponse> GetDepartment(int id)
        {
            // DepartmentViewModel rm = new DepartmentViewModel();
            var department = await _department.FindAsync(a => a.Id == id);
            if (department == null)
            {
                return new BaseResponse { Success = false, Message = "输入的部门编号不存在" };
            }
            var dto = _map.Map<DepartmentData>(department);
            await GetDepartmentChild(dto, department.Id);
            //rm.Id = department.Id;
            //rm.Success = true;
            //rm.Message = "获取部门信息成功";
            //rm.Name = department.DepartmentName;
            //rm.Level = department.Level;
            //rm.ParentId = department.ParentId;
            //rm.ParentPathIds = department.PathId;
            //rm.ParentPath = department.PathName;
            return new BResponse<DepartmentData> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetGroupDepartment(string GroupId)
        {
            //查找该组织的第一个部门
            var department = await _department.FindAsync(a => a.GroupId == GroupId && a.Parent == null);
            if (department == null)
            {
                return new BaseResponse { Success = false, Message = "输入的部门编号不存在" };
            }

            var dto = _map.Map<DepartmentData>(department);
            await GetDepartmentChild(dto, department.Id);
            return new BResponse<DepartmentData> { Success = true, Message = "获取数据成功", Data = dto };
        }

        public async Task GetDepartmentChild(DepartmentData da, int Id)
        {
            var data = await _department.Find(a => a.ParentId == Id).ToListAsync();
            if (data != null)
            {
                foreach (var item in data)
                {
                    var dto = _map.Map<DepartmentData>(item);
                    //da.Child = new List<DepartmentData>();
                    da.Child.Add(dto);
                    await GetDepartmentChild(dto, item.Id);
                }
            }
        }
        /// <summary>
        /// 查询部门信息，不能查询嵌套信息,如关联的角色和用户信息
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>返回是否有满足条件</returns>
        public async Task<bool> IsExist(Expression<Func<DepartmentModel, bool>> predicate)
        {
            var ret = await _department.Find(predicate).FirstOrDefaultAsync();
            if (ret != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> GetDepartmentGroupAsync(int Id)
        {
            var dept = await _department.FindAsync(Id);
            if (dept == null)
            {
                return null;
            }
            else
            {
                return dept.GroupId;
            }
        }

    }
}
