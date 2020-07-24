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
using Pipelines.Sockets.Unofficial.Arenas;

namespace HXCloud.Service
{
    public class TypeSystemAccessoryService : ITypeSystemAccessoryService
    {
        private readonly ILogger<TypeSystemAccessoryService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeSystemAccessoryRepository _tsr;

        public TypeSystemAccessoryService(ILogger<TypeSystemAccessoryService> log, IMapper mapper, ITypeSystemAccessoryRepository tsr)
        {
            this._log = log;
            this._mapper = mapper;
            this._tsr = tsr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeSystemAccessoryModel, bool>> predicate)
        {
            var data = await _tsr.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public bool IsExist(Expression<Func<TypeSystemAccessoryModel, bool>> predicate, out string GroupId)
        {
            var data = _tsr.FindWithSystem(predicate).Result;
            if (data == null)
            {
                GroupId = null;
                return false;
            }
            GroupId = data.TypeSystem.Type.GroupId;
            return true;
        }
        public async Task<BaseResponse> AddSystemAccessoryAsync(int systemId, TypeSystemAccessoryAddDto req, string account)
        {
            //检查配件是否重名
            var data = await _tsr.Find(a => a.SystemId == systemId && a.Name == req.Name).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该子系统下已存在相同名称的配件" };
            }
            try
            {
                var entity = _mapper.Map<TypeSystemAccessoryModel>(req);
                entity.Create = account;
                entity.SystemId = systemId;
                await _tsr.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}的子系统配件成功");
                return new HandleResponse<int> { Success = true, Message = "添加子系统配件成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加子系统配件失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型子系统配件失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateTypeSystemAccessoryAsync(int systemId, TypeSystemAccessoryUpdateDto req, string account)
        {
            var data = await _tsr.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型配件不存在" };
            }
            var acc = await _tsr.Find(a => a.SystemId == systemId && a.Name == req.Name).FirstOrDefaultAsync();
            if (acc != null && acc.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "该系统下已存在相同名称的配件" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _tsr.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的系统配件成功");
                return new BaseResponse { Success = true, Message = "修改系统配件成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的系统配件失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改系统配件失败" };
            }
        }

        public async Task<BaseResponse> DeleteSystemAccessoryAsync(int Id, string account)
        {
            var data = await _tsr.FindWithControlData(a => a.Id == Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的子系统配件不存在" };
            }
            if (data.TypeSystemAccessoryControlDatas.Count > 0)
            {
                return new BaseResponse { Success = false, Message = "该子系统配件下存在控制数据，不能删除" };
            }
            try
            {
                await _tsr.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的类型配件成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的类型配件失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败,请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetAccessoryAsync(int Id)
        {
            var data = await _tsr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的配件编号不存在" };
            }
            var dto = _mapper.Map<TypeSystemAccessoryDto>(data);
            return new BResponse<TypeSystemAccessoryDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetSystemAccessory(int systemId, BasePageRequest req)
        {
            var query = _tsr.Find(a => a.SystemId == systemId);
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
            var dtos = _mapper.Map<List<TypeSystemAccessoryDto>>(data);
            return new BasePageResponse<List<TypeSystemAccessoryDto>>
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
