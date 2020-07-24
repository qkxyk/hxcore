using System;
using System.Collections.Generic;
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
using System.Linq;

namespace HXCloud.Service
{
    public class TypeConfigService : ITypeConfigService
    {
        private readonly ILogger<TypeConfigService> _log;
        private readonly ITypeConfigRepository _tc;
        private readonly IMapper _mapper;
        private readonly ITypeRepository _tr;

        public TypeConfigService(ILogger<TypeConfigService> log, ITypeConfigRepository tc, IMapper mapper, ITypeRepository tr)
        {
            this._log = log;
            this._tc = tc;
            this._mapper = mapper;
            this._tr = tr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeConfigModel, bool>> predicate)
        {
            var data = await _tc.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public bool IsExist(Expression<Func<TypeConfigModel, bool>> predicate, out string GroupId)
        {
            var data = _tc.FindWithType(predicate).Result;
            if (data == null)
            {
                GroupId = null;
                return false;
            }
            GroupId = data.Type.GroupId;
            return true;
        }

        public async Task<BaseResponse> AddAsync(int typeId, TypeConfigAddViewModel req, string account)
        {
            //验证类型是否可以添加
            var t = await _tr.FindAsync(typeId);
            if (t.Status == TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            //验证是否重名
            var data = await _tc.Find(a => a.TypeId == typeId && a.DataName == req.DataName).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = true, Message = "已存在相同名称的类型配置，请确认" };
            }
            try
            {
                var entity = _mapper.Map<TypeConfigModel>(req);
                entity.Create = account;
                entity.TypeId = typeId;
                await _tc.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}的类型配置数据成功");
                return new HandleResponse<int> { Success = true, Message = "添加类型配置数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加类型配置数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型配置数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateAsync(TypeConfigUpdateViewModel req, string account)
        {
            var data = await _tc.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型配置不存在" };
            }
            var ret = await _tc.Find(a => a.DataName == req.DataName && a.TypeId == data.TypeId).FirstOrDefaultAsync();
            if (ret != null && ret.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的类型配置" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _tc.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的类型配置数据成功");
                return new BaseResponse { Success = true, Message = "修改类型配置数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的类型配置数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改类型配置数据失败" };
            }
        }

        public async Task<BaseResponse> DeleteAsync(int Id, string account)
        {
            var data = await _tc.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型配置标示不存在" };
            }
            try
            {
                await _tc.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的类型配置数据成功");
                return new BaseResponse { Success = true, Message = "删除类型配置数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的类型配置数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除类型配置数据失败" };
            }
        }

        public async Task<BaseResponse> FindById(int Id)
        {
            var data = await _tc.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型配置编号不存在" };
            }
            var dto = _mapper.Map<TypeConfigData>(data);
            return new BResponse<TypeConfigData> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> FindByType(int typeId, TypeConfigPageRequestViewModel req)
        {
            var query = _tc.Find(a => a.TypeId == typeId);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                query = query.Where(a => a.DataName.Contains(req.Search) || a.DataType.Contains(req.Search) || a.DataValue.Contains(req.Search));
            }
            int Count = query.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
                //UserQuery = UserQuery.OrderBy(a => a.Id);
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var data = await query.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<TypeConfigData>>(data);
            return new BasePageResponse<List<TypeConfigData>>
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
    }
}
