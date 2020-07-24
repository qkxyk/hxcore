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
    public class TypeAccessoryControlDataService : ITypeAccessoryControlDataService
    {
        private readonly ILogger<TypeAccessoryControlDataService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeAccessoryControlDataRepository _tacr;

        public TypeAccessoryControlDataService(ILogger<TypeAccessoryControlDataService> log, IMapper mapper, ITypeAccessoryControlDataRepository tacr)
        {
            this._log = log;
            this._mapper = mapper;
            this._tacr = tacr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeAccessoryControlDataModel, bool>> predicate)
        {
            var data = await _tacr.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public bool IsExist(Expression<Func<TypeAccessoryControlDataModel, bool>> predicate, out string GroupId)
        {
            var data = _tacr.FindWithType(predicate).Result;
            if (data == null)
            {
                GroupId = null;
                return false;
            }
            GroupId = data.TypeAccessory.Type.GroupId;
            return true;
        }
        public async Task<BaseResponse> AddAccessoryControlData(int accessoryId, TypeControlDataAddDto req, string account)
        {
            var data = await _tacr.Find(a => a.AccessoryId == accessoryId && a.ControlName == req.ControlName).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的控制数据" };
            }
            try
            {
                var entity = _mapper.Map<TypeAccessoryControlDataModel>(req);
                entity.Create = account;
                entity.AccessoryId = accessoryId;
                await _tacr.AddAsync(entity);
                _log.LogInformation($"{account}创建类型配件控制数据成功");
                return new HandleResponse<int> { Success = true, Message = "创建类型配件控制数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}创建类型配件控制数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "创建类型配件控制数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateTypeControlDataAsync(int accessoryId, TypeControlDataUpdateDto req, string account)
        {
            var data = await _tacr.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型配件控制数据不存在" };
            }
            var control = await _tacr.Find(a => a.AccessoryId == accessoryId && a.ControlName == req.ControlName).FirstOrDefaultAsync();
            if (control != null && control.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "该类型配件先已存在相同名称的控制数据" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _tacr.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的类型配件控制数据成功");
                return new BaseResponse { Success = false, Message = "修改类型配件控制数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}标示为{req.Id}的类型配件控制数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改类型配件控制数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteTypeAccessoryControlDataAsync(int Id, string account)
        {
            var data = await _tacr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型配件控制数据不存在" };
            }
            try
            {
                await _tacr.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的类型配件控制数据成功");
                return new BaseResponse { Success = true, Message = "删除类型配件控制数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的类型配件控制数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除类型配件控制数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetControlDataAsync(int Id)
        {
            var data = await _tacr.FindAsync(Id);
            if (data==null)
            {
                return new BaseResponse { Success = false, Message = "输入的配件标示不存在" };
            }
            var dto = _mapper.Map<TypeControlDataDto>(data);
            return new BResponse<TypeControlDataDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetAccessoryControlDataAsync(int accessoryId)
        {
            var data = await _tacr.Find(a => a.AccessoryId == accessoryId).ToListAsync();
            var dto = _mapper.Map<List<TypeControlDataDto>>(data);
            return new BResponse<List<TypeControlDataDto>> { Success = true, Message = "获取数据成功", Data = dto };
        }
    }
}
