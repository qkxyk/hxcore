using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class TypeModuleArgumentService : ITypeModuleArgumentService
    {
        private readonly ILogger<TypeModuleArgumentModel> _log;
        private readonly ITypeModuleArgumentRepository _tma;
        private readonly ITypeDataDefineRepository _tdr;
        private readonly IMapper _map;
        private readonly ITypeModuleRepository _tmr;

        public TypeModuleArgumentService(ILogger<TypeModuleArgumentModel> log, ITypeModuleArgumentRepository tma, ITypeDataDefineRepository tdr, IMapper map, ITypeModuleRepository tmr)
        {
            this._log = log;
            this._tma = tma;
            this._tdr = tdr;
            this._map = map;
            this._tmr = tmr;
        }
        public async Task<TypeModuleArgumentCheckDto> IsExistCheck(Expression<Func<TypeModuleModel, bool>> predicate)
        {
            TypeModuleArgumentCheckDto ret = new TypeModuleArgumentCheckDto();
            var data = await _tmr.FindWithTypeAsync(predicate);
            if (data == null)
            {
                ret.IsExist = false;
            }
            else
            {
                ret.IsExist = true;
                ret.GroupId = data.Type.GroupId;
                ret.TypeId = data.TypeId;
            }
            return ret;
        }

        public async Task<bool> IsExist(Expression<Func<TypeModuleModel, bool>> predicate)
        {
            var count = await _tmr.Find(predicate).CountAsync();
            if (count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加模块配置项数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="ModuleId">模块标识</param>
        /// <param name="req">配置项数据</param>
        /// <returns>返回操作数据</returns>
        public async Task<BaseResponse> AddTypeModuleArgumentAsync(string Account, int ModuleId, int TypeId, TypeModuleArgumentAddDto req)
        {
            var data = await _tma.FindAsync(a => a.ModuleId == ModuleId && a.Name == req.Name);
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该模块下已存在相同名称的配置数据" };
            }
            //查看该类型下是否存在该数据定义
            var count = await _tdr.Find(a => a.Id == req.DataDefineId && a.TypeId == TypeId).CountAsync();
            if (count == 0)
            {
                return new BaseResponse { Success = false, Message = $"该类型下不存在{req.DataDefineId}的数据定义" };
            }
            try
            {
                var entity = _map.Map<TypeModuleArgumentModel>(req);
                entity.Create = Account;
                entity.ModuleId = ModuleId;
                await _tma.AddAsync(entity);
                _log.LogInformation($"{Account}添加标示为{entity.Id}的模块配置项成功");
                return new HandleResponse<int> { Key = entity.Id, Success = true, Message = "添加数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加模块标示为{ModuleId}的模块配置数据失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 更改模块的配置数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="ModuleId">模块标识</param>
        /// <param name="req">模块配置数据</param>
        /// <returns>返回修改模块配置数据是否成功</returns>
        public async Task<BaseResponse> UpdateModuleArgumentAsync(string Account, int ModuleId, int TypeId, TypeModuleArgumentUpdateDto req)
        {
            var entity = await _tma.FindAsync(req.Id);
            if (entity == null)
            {
                return new BaseResponse { Success = false, Message = "该模块下不存在此配置数据" };
            }
            var data = await _tma.FindAsync(a => a.ModuleId == ModuleId && a.Name == req.Name);
            if (data != null && data.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "该模块下已存在相同名称的配置数据" };
            }
            //查看该类型下是否存在该数据定义
            var count = await _tdr.Find(a => a.Id == req.DataDefineId && a.TypeId == TypeId).CountAsync();
            if (count == 0)
            {
                return new BaseResponse { Success = false, Message = $"该类型下不存在{req.DataDefineId}的数据定义" };
            }
            try
            {
                _map.Map(req, entity);
                entity.Modify = Account;
                entity.ModifyTime = DateTime.Now;
                await _tma.SaveAsync(entity);
                _log.LogInformation($"{Account}修改标示为{req.Id}的模块配置数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改标示为{req.Id}的模块配置数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 删除模块配置数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="Id">模块配置数据标识</param>
        /// <returns>返回删除数据是否成功信息</returns>
        public async Task<BaseResponse> DeleteTypeModuleArgumentAsync(string Account, int Id)
        {
            var data = await _tma.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "该配置已删除，请勿重复删除" };
            }
            try
            {
                await _tma.RemoveAsync(data);
                _log.LogInformation($"{Account}删除标示为{Id}的模块配置数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}删除标示为{Id}的模块控制数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 根据模块标识获取模块配置数据
        /// </summary>
        /// <param name="ModuleId">模块标识</param>
        /// <returns>获取模块下的配置数据</returns>
        public async Task<BaseResponse> GetTypeModuleArgumentAsync(int ModuleId)
        {
            var data = await _tma.FindWithTypeDataDefineAsync(a => a.ModuleId == ModuleId);
            var dtos = _map.Map<List<TypeModuleArgumentDto>>(data);
            return new BResponse<List<TypeModuleArgumentDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }


    }
}
