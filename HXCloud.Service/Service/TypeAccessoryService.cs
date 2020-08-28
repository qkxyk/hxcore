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
using System.Linq.Dynamic.Core;

namespace HXCloud.Service
{
    public class TypeAccessoryService : ITypeAccessoryService
    {
        private readonly ILogger<TypeAccessoryService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeAccessoryRepository _tar;
        private readonly ITypeRepository _tr;

        public TypeAccessoryService(ILogger<TypeAccessoryService> log, IMapper mapper, ITypeAccessoryRepository tar, ITypeRepository tr)
        {
            this._log = log;
            this._mapper = mapper;
            this._tar = tar;
            this._tr = tr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeAccessoryModel, bool>> predicate)
        {
            var data = await _tar.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public bool IsExist(Expression<Func<TypeAccessoryModel, bool>> predicate, out string GroupId)
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
        public async Task<BaseResponse> AddAccessoryAsync(int typeId, TypeAccessoryAddViewModel req, string account)
        {
            //验证类型是否可以添加
            var t = await _tr.FindAsync(typeId);
            if (t.Status == TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            //验证同一个类型下是否添加相同的配件
            var data = await _tar.Find(a => a.TypeId == typeId && a.Name == req.Name).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该类型下已存在相同名称的配件，请确认" };
            }
            try
            {
                var entity = _mapper.Map<TypeAccessoryModel>(req);
                entity.Create = account;
                entity.TypeId = typeId;
                await _tar.AddAsync(entity);
                _log.LogInformation($"{account}添加类型配件{entity.Id}的类型配件成功");
                return new HandleResponse<int> { Success = true, Message = "添加类型配件成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加类型配件失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型配件失败，请联系管理员!" };
            }
        }
        public async Task<BaseResponse> UpdateTypeAccessoryAsync(int typeId, TypeAccessoryUpdateViewModel req, string account)
        {
            var data = await _tar.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型配件不存在" };
            }
            var accessory = await _tar.Find(a => a.TypeId == typeId && a.Name == req.Name && a.Id != req.Id).FirstOrDefaultAsync();
            if (accessory != null)
            {
                return new BaseResponse { Success = false, Message = "该类型下已存在相同的配件名称" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _tar.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的类型配件数据成功");
                return new BaseResponse { Success = true, Message = "修改类型配件成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的类型配件数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改类型配件失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> DeleteTypeAccessoryAsync(int Id, string account)
        {
            var data = await _tar.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型配件不存在" };
            }
            try
            {
                await _tar.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的类型配件成功");
                return new BaseResponse { Success = true, Message = "删除类型配件成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的类型配件失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除类型配件失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetAccessoryAsync(int Id)
        {
            var data = await _tar.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型配件不存在" };
            }
            var dto = _mapper.Map<TypeAccessoryData>(data);
            return new BResponse<TypeAccessoryData> { Success = true, Message = "获取数据成功", Data = dto };
        }

        public async Task<BaseResponse> GetTypeAccessoryAsync(int typeId, BasePageRequest req)
        {
            var query = _tar.Find(a => a.TypeId == typeId);
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
            var dtos = _mapper.Map<List<TypeAccessoryData>>(data);
            return new BasePageResponse<List<TypeAccessoryData>>
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


        public async Task<BaseResponse> GetTypeAccessoryAndControlDataAsync(int TypeId)
        {
            var data =await _tar.FindWithControlData(TypeId);
            var dtos = _mapper.Map<List<TypeAccessoryDto>>(data);
            return new BResponse<List<TypeAccessoryDto>> { Success = true, Message = "获取数据成功", Data = dtos };

        }
    }
}
