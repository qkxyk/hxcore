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
    public class TypeModuleControlService : ITypeModuleControlService
    {
        private readonly ILogger<TypeModuleControlService> _log;
        private readonly IMapper _map;
        private readonly ITypeModuleControlRepository _tmcr;
        private readonly ITypeDataDefineRepository _tdr;

        public TypeModuleControlService(ILogger<TypeModuleControlService> log, IMapper map, ITypeModuleControlRepository tmcr, ITypeDataDefineRepository tdr)
        {
            this._log = log;
            this._map = map;
            this._tmcr = tmcr;
            this._tdr = tdr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeModuleControlModel, bool>> predicate)
        {
            var data = await _tmcr.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }

        public async Task<BaseResponse> AddTypeModuleControlAsync(string Account/*, int ModuleId*/, TypeModuleControlAddDto req)
        {
            var count = await _tmcr.Find(a => a.ModuleId == req.ModuleId && a.ControlName == req.ControlName).CountAsync();
            if (count > 0)
            {
                return new BaseResponse { Success = false, Message = "此模块下已添加相同名称的控制项" };
            }
            try
            {
                var entity = _map.Map<TypeModuleControlModel>(req);
                entity.Create = Account;
                //entity.ModuleId = reqModuleId;
                await _tmcr.AddAsync(entity);
                _log.LogInformation($"{Account}添加标示为{entity.Id}的控制项成功");
                return new HandleResponse<int> { Success = true, Message = "添加数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加模块标示为{req.ModuleId}的控制项失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateTypeModuleControlAsync(string Account,/* int ModuleId,*/ TypeModuleControlUpdateDto req)
        {
            var data = await _tmcr.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的控制项不存在" };
            }
            var count = await _tmcr.Find(a => a.ModuleId == data.ModuleId && a.ControlName == req.ControlName && a.Id != req.Id).CountAsync();
            if (count > 0)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的控制项" };
            }
            try
            {
                var entity = _map.Map(req, data);
                entity.Modify = Account;
                entity.ModifyTime = DateTime.Now;
                await _tmcr.SaveAsync(entity);
                _log.LogInformation($"{Account}修改标示为{req.Id}的控制项成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改标示为{req.Id}的控制项失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteTypeModuleControlAsync(string Account, int Id)
        {
            var data = await _tmcr.FindWithFeedbackAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "该控制项已删除，请勿重复删除" };
            }
            if (data.TypeModuleFeedbacks.Count > 0)
            {
                return new BaseResponse { Success = false, Message = "该控制项下存在关联的反馈数据，不能删除" };
            }
            try
            {
                await _tmcr.RemoveAsync(data);
                _log.LogInformation($"{Account}删除标示为{Id}的控制项成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}删除标示为{Id}的控制项失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetTypeModuleControlsByModuleIdAsync(int ModuleId)
        {
            var data = await _tmcr.FindWithFeedbackAndDataDefineAsync(a => a.ModuleId == ModuleId);
            var dtos = _map.Map<IEnumerable<TypeModuleControlDto>>(data);
            return new BResponse<IEnumerable<TypeModuleControlDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }

        public async Task<BaseResponse> GetTypeModuleControlsByIdAsync(int Id)
        {
            var data = await _tmcr.FindWithFeedbackAndDataDefineAsync(a => a.Id == Id);
            var dto = _map.Map<TypeModuleControlDto>(data.FirstOrDefault());
            return new BResponse<TypeModuleControlDto> { Success = true, Message = "获取数据成功", Data = dto };
        }

        public async Task<TypeModuleControlCheckDto> IsExistCheck(Expression<Func<TypeModuleControlModel, bool>> predicate)
        {
            TypeModuleControlCheckDto dto = new TypeModuleControlCheckDto();
            var data = await _tmcr.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                dto.IsExist = false;
            }
            else
            {
                dto.IsExist = true;
                dto.ModuleId = data.ModuleId;
            }
            return dto;
        }
    }
}
