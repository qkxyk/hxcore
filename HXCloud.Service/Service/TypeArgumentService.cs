using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
    public class TypeArgumentService : ITypeArgumentService
    {
        private readonly ILogger<TypeArgumentService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeArgumentRepository _tar;
        private readonly ITypeRepository _tr;

        public TypeArgumentService(ILogger<TypeArgumentService> log, IMapper mapper, ITypeArgumentRepository tar, ITypeRepository tr)
        {
            this._log = log;
            this._mapper = mapper;
            this._tar = tar;
            this._tr = tr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeArgumentModel, bool>> predicate)
        {
            var data = await _tar.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public bool IsExist(Expression<Func<TypeArgumentModel, bool>> predicate, out string GroupId)
        {
            var data = _tar.FindWithType(predicate).Result;
            if (data == null)
            {
                GroupId = null;
                return false;
            }
            GroupId = data.Type.GroupId;
            return true;
        }
        public async Task<BaseResponse> AddArgumentAsync(int typeId, TypeArgumentAddViewModel req, string account)
        {
            //验证类型是否可以添加
            var t = await _tr.FindAsync(typeId);
            if (t.Status == TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            //验证是否重名
            var data = await _tar.Find(a => a.Name == req.Name && a.TypeId == typeId).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该类型下已存在相同的配置数据" };
            }
            try
            {
                var entity = _mapper.Map<TypeArgumentModel>(req);
                entity.Create = account;
                entity.TypeId = typeId;
                await _tar.AddAsync(entity);
                _log.LogInformation($"{account}添加类型配置{entity.Id}成功");
                return new HandleResponse<int> { Success = true, Message = "添加类型配置数据成", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加类型配置数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型配置数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateTypeArgumentAsync(int typeId, TypeArgumentUpdateViewModel req, string account)
        {
            var data = await _tar.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型配置数据不存在" };
            }
            var arg = await _tar.Find(a => a.TypeId == typeId && a.Name == req.Name).FirstOrDefaultAsync();
            if (arg != null && arg.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "该类型下已存在相同名称的类型配置数据" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _tar.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的类型配置数据成功");
                return new BaseResponse { Success = true, Message = "修改类型配置数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的类型配置数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改类型配置数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteTypeArgumentAsync(int Id, string account)
        {
            var data = await _tar.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型配置信息不存在" };
            }
            try
            {
                await _tar.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的类型配置数据成功");
                return new BaseResponse { Success = false, Message = "删除类型配置数据失败" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的类型配置数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除类型配置数据失败,请联系管理员 " };
            }
        }
        public async Task<BaseResponse> GetArgumentAsync(int Id)
        {
            var data = await _tar.FindWithDataDefine(a => a.Id == Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型配置数据不存在" };
            }
            var dto = _mapper.Map<TypeArgumentDto>(data);
            return new BResponse<TypeArgumentDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetTypeArgumentAsync(int typeId, BasePageRequest req)
        {
            var query = _tar.FindWithDataDefines(a => a.TypeId == typeId);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                query = query.Where(a => a.Name.Contains(req.Search));
            }
            int Count = query.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var data = await query.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<TypeArgumentDto>>(data);
            return new BasePageResponse<List<TypeArgumentDto>>
            {
                Success = true,
                Message = "获取数据成功",
                Count = Count,
                CurrentPage = req.PageNo,
                PageSize = req.PageSize,
                TotalPage = (int)Math.Ceiling((decimal)Count / req.PageSize),
                Data = dtos
            };
        }

        public async Task<BaseResponse> GetTypeArgumentByCategory(int TypeId, string Category)
        {
            var data = await _tar.FindWithDataDefine(a => a.TypeId == TypeId && a.Category == Category);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类别不存在" };
            }
            var dto = _mapper.Map<TypeArgumentDto>(data);
            return new BResponse<TypeArgumentDto> { Success = true, Message = "获取数据成", Data = dto };
        }
    }
}
