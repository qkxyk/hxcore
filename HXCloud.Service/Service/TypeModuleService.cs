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
    public class TypeModuleService : ITypeModuleService
    {
        private readonly ILogger<TypeModuleService> _log;
        private readonly IMapper _map;
        private readonly ITypeModuleRepository _tmr;
        private readonly ITypeRepository _tr;
        private readonly ITypeModuleArgumentRepository _tar;

        public TypeModuleService(ILogger<TypeModuleService> log, IMapper map, ITypeModuleRepository tmr, ITypeRepository tr, ITypeModuleArgumentRepository tar)
        {
            this._log = log;
            this._map = map;
            this._tmr = tmr;
            this._tr = tr;
            this._tar = tar;
        }
        public async Task<bool> IsExist(Expression<Func<TypeModuleModel, bool>> predicate)
        {
            var data = await _tmr.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public async Task<BaseResponse> AddTypeModuleAsync(string Account, int TypeId, TypeModuleAddDto req)
        {
            //验证是否重名
            var m = await _tmr.Find(a => a.TypeId == TypeId && a.ModuleName == req.ModuleName).CountAsync();
            if (m > 0)
            {
                return new BaseResponse { Success = false, Message = "该类型下存在相同的模块名称" };
            }
            try
            {
                var entity = _map.Map<TypeModuleModel>(req);
                entity.Create = Account;
                entity.TypeId = TypeId;
                await _tmr.AddAsync(entity);
                _log.LogInformation($"{Account}添加标示为{entity.Id}的类型模块成功");
                return new HandleResponse<int> { Success = true, Message = "添加模块成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加类型模块失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateTypeModuleAsync(string Account, int TypeId, TypeModuleUpdateDto req)
        {
            var m = await _tmr.FindAsync(req.Id);
            if (m == null)
            {
                return new BaseResponse { Success = false, Message = "数据的模块不存在" };
            }
            var count = await _tmr.Find(a => a.TypeId == TypeId && a.ModuleName == req.ModuleName && a.Id != req.Id).CountAsync();
            if (count > 0)
            {
                return new BaseResponse { Success = false, Message = "该类型下已存在相同名称的模块" };
            }
            try
            {
                var entity = _map.Map(req, m);
                entity.Modify = Account;
                entity.ModifyTime = DateTime.Now;
                await _tmr.SaveAsync(entity);
                _log.LogInformation($"{Account}修改标示为{req.Id}的模块数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改标示为{req.Id}的类型模块数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteTypeModuleAsync(string Account, int Id)
        {
            var m = await _tmr.FindWithControlAsync(Id);
            if (m == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型模块不存在" };
            }
            if (m.ModuleControls.Count > 0)
            {
                return new BaseResponse { Success = false, Message = "该模块下存在相关的控制数据，不能删除" };
            }
            if (m.ModeleArguments.Count > 0)
            {
                return new BaseResponse { Success = false, Message = "该模块下存在相关的配置数据，不能删除" };
            }
            try
            {
                await _tmr.RemoveAsync(m);
                _log.LogInformation($"{Account}删除标示为{Id}的类型模块成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}删除标示为{Id}的模块失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetTypeModulesByTypeIdAsync(int TypeId)
        {
            var data = await _tmr.FindWithControlAndFeedbackAsync(a => a.TypeId == TypeId);
            var dtos = _map.Map<IEnumerable<TypeModuleDto>>(data);
            foreach (var item in dtos)
            {
                var dt = await _tar.FindWithTypeDataDefineAsync(a => a.ModuleId == item.Id);
                item.Arguments = _map.Map<List<TypeModuleArgumentDto>>(dt);
            }
            return new BResponse<IEnumerable<TypeModuleDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
        public async Task<BaseResponse> GetTypeModuleByIdAsync(int Id)
        {
            var data = await _tmr.FindWithControlAndFeedbackAsync(a => a.Id == Id);
            var dto = _map.Map<TypeModuleDto>(data.FirstOrDefault());
            var dt = await _tar.FindWithTypeDataDefineAsync(a => a.ModuleId == Id);
            dto.Arguments = _map.Map<List<TypeModuleArgumentDto>>(dt);
            return new BResponse<TypeModuleDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
    }
}
