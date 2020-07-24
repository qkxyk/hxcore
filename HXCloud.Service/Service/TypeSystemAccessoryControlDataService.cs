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
    public class TypeSystemAccessoryControlDataService : ITypeSystemAccessoryControlDataService
    {
        private readonly ILogger<TypeSystemAccessoryControlDataService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeSystemAccessoryControlDataRepository _tscr;

        public TypeSystemAccessoryControlDataService(ILogger<TypeSystemAccessoryControlDataService> log, IMapper mapper, ITypeSystemAccessoryControlDataRepository tscr)
        {
            this._log = log;
            this._mapper = mapper;
            this._tscr = tscr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeSystemAccessoryControlDataModel, bool>> predicate)
        {
            var data = await _tscr.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public async Task<BaseResponse> AddSystemControlDataAsync(int accessoryId, TypeSystemAccessoryControlDataAddDto req, string account)
        {
            var data = await _tscr.Find(a => a.AccessoryId == accessoryId && a.ControlName == req.ControlName).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该配件下已存在相同名称的控制数据" };
            }
            try
            {
                var entity = _mapper.Map<TypeSystemAccessoryControlDataModel>(req);
                entity.AccessoryId = accessoryId;
                entity.Create = account;
                await _tscr.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}的类型配件控制数据成功");
                return new HandleResponse<int> { Success = true, Message = "添加控制数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加类型配件控制数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型配件控制数据失败" };
            }
        }

        public async Task<BaseResponse> UpdateSystemAccessoryControlDataAsync(int accessoryId, TypeSystemAccessoryControlDataUpdateDto req, string account)
        {
            var data = await _tscr.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的子系统配件控制数据不存在" };
            }
            var control = await _tscr.Find(a => a.AccessoryId == accessoryId && a.ControlName == req.ControlName).FirstOrDefaultAsync();
            if (control != null && control.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "该配件下已存在相同名称的控制数据" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _tscr.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的子系统配件控制数据成功");
                return new BaseResponse { Success = true, Message = "修改子系统配件控制数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的子系统配件控制数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改子系统配件控制数据失败" };
            }
        }

        public async Task<BaseResponse> DeleteSystemAccessoryControlDataAsync(int Id, string account)
        {
            var data = await _tscr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的子系统配件控制数据不存在" };
            }
            try
            {
                await _tscr.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的子系统配件控制数据成功");
                return new BaseResponse { Success = false, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的子系统类型配件控制数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetControlDataAsync(int Id)
        {
            var data = await _tscr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的配件控制数据标示不存在" };
            }
            var dto = _mapper.Map<TypeSystemAccessoryControlDataDto>(data);
            return new BResponse<TypeSystemAccessoryControlDataDto> { Success = true, Message = "获取数据成功", Data = dto };
        }

        public async Task<BaseResponse> GetAccessoryControlDataAsync(int accessoryId)
        {
            var data = await _tscr.Find(a => a.AccessoryId == accessoryId).ToListAsync();
            var dto = _mapper.Map<List<TypeSystemAccessoryControlDataDto>>(data);
            return new BResponse<List<TypeSystemAccessoryControlDataDto>> { Success = true, Message = "获取数据成功", Data = dto };
        }
    }
}
