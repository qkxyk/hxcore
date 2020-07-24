using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HXCloud.Service
{
    //导入组织类型数据功能因功能复杂待商榷
    public class TypeService : ITypeService
    {
        private readonly ILogger<TypeService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeRepository _tr;

        public TypeService(ILogger<TypeService> log, IMapper mapper, ITypeRepository tr)
        {
            this._log = log;
            this._mapper = mapper;
            this._tr = tr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeModel, bool>> predicate)
        {
            var ret = await _tr.Find(predicate).FirstOrDefaultAsync();
            if (ret == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 验证类型是否存在，并且获取类型所在的组织
        /// </summary>
        /// <param name="Id">类型标示</param>
        /// <param name="GroupId">类型所属组织编号</param>
        /// <returns>类型是否存在</returns>
        public bool IsExist(int Id, out string GroupId)
        {
            var data = _tr.Find(Id);
            if (data == null)
            {
                GroupId = null;
                return false;
            }
            GroupId = data.GroupId;
            return true;
        }
        /// <summary>
        /// 验证该类型下是否能添加相关的信息
        /// </summary>
        /// <param name="Id">类型标示</param>
        /// <param name="GroupId">类型所属的组织编号</param>
        /// <param name="status">类型是否能是叶子节点</param>
        /// <returns>类型是否存在</returns>
        public bool IsExist(int Id, out string GroupId, out int status)
        {
            var data = _tr.Find(Id);
            if (data == null)
            {
                GroupId = null;
                status = 0;
                return false;
            }
            GroupId = data.GroupId;
            status = (int)data.Status;
            return true;
        }
        public bool IsExist(Expression<Func<TypeModel, bool>> predicate, out string GroupId)
        {
            var ret = _tr.Find(predicate).FirstOrDefault();
            if (ret == null)
            {
                GroupId = null;
                return false;
            }
            GroupId = ret.GroupId;
            return true;
        }
        public async Task<BaseResponse> AddType(TypeAddViewModel req, string account)
        {
            //类型可以添加多个顶级类型
            string pathId = null, pathName = null;
            if (req.ParentId.HasValue && req.ParentId.Value != 0)
            {
                var parent = await _tr.FindAsync(req.ParentId.Value);
                if (parent == null)
                {
                    return new BaseResponse { Success = false, Message = "输入的父类型不存在" };
                }
                if (parent.Status == TypeStatus.Leaf)
                {
                    return new BaseResponse { Success = false, Message = "数据节点下不能添加节点" };
                }
                if (parent.ParentId == null)
                {
                    pathId = parent.Id.ToString();
                    pathName = parent.TypeName;
                }
                else
                {
                    pathId = $"{ parent.PathId}/{parent.Id}";
                    pathName = $"{parent.PathName}/{parent.TypeName}";
                }
            }
            else//顶级节点
            {
                if (req.Status == 1)      //不能直接添加数据节点
                {
                    return new BaseResponse { Success = false, Message = "数据节点必须添加在目录节点下" };
                }
            }
            //检查是否重名
            var ret = await _tr.Find(a => a.ParentId == req.ParentId && a.TypeName == req.TypeName).FirstOrDefaultAsync();
            if (ret != null)
            {
                return new BaseResponse { Success = false, Message = "该节点下已存在相同类型名称" };
            }
            var tm = _mapper.Map<TypeModel>(req);
            try
            {
                tm.Create = account;
                tm.PathId = pathId;
                tm.PathName = pathName;
                await _tr.AddAsync(tm);
                _log.LogInformation($"{account}添加类型{tm.Id}->{tm.TypeName}成功");
                return new HandleResponse<int> { Success = true, Message = "添加类型成功", Key = tm.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加类型{req.TypeName}失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateType(TypeUpdateViewModel req, string account)
        {
            var dm = await _tr.FindAsync(req.Id);
            if (dm == null)
            {
                return new BaseResponse { Success = false, Message = "该类型不存在" };
            }
            //同一部门下的子部门不能重名
            var d = await _tr.Find(a => a.ParentId == dm.ParentId && a.TypeName == req.TypeName && a.GroupId == dm.GroupId).FirstOrDefaultAsync();
            if (d != null && d.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "该组织下存在相同的类型名称" };
            }
            try
            {
                dm.Modify = account;
                dm.ModifyTime = DateTime.Now;
                _mapper.Map(req, dm);
                await _tr.SaveAsync(dm);
                _log.LogInformation($"{account}修改Id为{req.Id}的类型名称为{req.TypeName}成功");
                return new BaseResponse { Success = true, Message = "修改类型数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改id为{req.Id}的类型名称失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改类型数据失败" };
            }
        }
        public async Task<BaseResponse> DeleteTypeAsync(int Id, string account)
        {
            //验证是否存在子类型
            var ret = await _tr.FindAsync(a => a.Id == Id);
            if (ret.Child.Count != 0)
            {
                return new BaseResponse { Success = false, Message = "该类型下存在子类型，不能删除" };
            }
            try
            {
                await _tr.RemoveAsync(ret);
                _log.LogInformation($"{account}删除类型标示为{ret.Id}的类型数据成功");
                return new BaseResponse { Success = true, Message = "删除类型成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除类型标示为{ret.Id}的类型失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除类型数据失败" };
            }

        }
        /*
        //此功能暂缓
        public async Task<BaseResponse> UpdateTypeIcon(string Icon, string account)
        {
            throw new NotImplementedException();
        }
*/
        /// <summary>
        /// 获取类型及其子节点
        /// </summary>
        /// <param name="Id">类型标示</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetTypeAsync(int Id)
        {
            var ret = await _tr.FindAsync(a => a.Id == Id);
            var dto = _mapper.Map<TypeData>(ret);
            if (ret.Child.Count > 0)            //如果有子节点，则递归获取
            {
                await GetChild(dto, Id);
            }
            return new BResponse<TypeData> { Success = true, Message = "获取类型数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetGroupTypeAsync(string GroupId)
        {
            var ret = await _tr.Find(a => a.ParentId == null && a.GroupId == GroupId).ToListAsync();
            List<TypeData> dtos = new List<TypeData>();
            dtos = _mapper.Map<List<TypeData>>(ret);
            foreach (var item in dtos)
            {
                await GetChild(item, item.Id);
            }
            //foreach (var item in ret)
            //{
            //    var dto = _mapper.Map<TypeData>(item);
            //    dtos.Add(dto);
            //    await GetChild(dto, item.Id);
            //}
            return new BResponse<List<TypeData>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
        public async Task GetChild(TypeData td, int Id)
        {
            var ret = await _tr.Find(a => a.ParentId == Id).ToListAsync();
            foreach (var item in ret)
            {
                var dto = _mapper.Map<TypeData>(item);
                td.Child.Add(dto);
                await GetChild(dto, item.Id);
            }
        }

        public async Task<string> GetTypeGroupIdAsync(int Id)
        {
            var ret = await _tr.FindAsync(Id);
            if (ret == null)
            {
                return null;
            }
            return ret.GroupId;
        }
    }
}
