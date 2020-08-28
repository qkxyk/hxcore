using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class WarnTypeService : IWarnTypeService
    {
        private readonly ILogger _log;
        private readonly IMapper _mapper;
        private readonly IWarnTypeRepository _warnType;

        public WarnTypeService(ILogger log, IMapper mapper, IWarnTypeRepository warnType)
        {
            this._log = log;
            this._mapper = mapper;
            this._warnType = warnType;
        }
        public Task<bool> IsExist(Expression<Func<WarnTypeModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> AddWarnTypeAsync(string account, WarnTypeAddDto req)
        {
            var type = await _warnType.Find(a => a.TypeName == req.TypeName).FirstOrDefaultAsync();
            if (type != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的类型" };
            }
            try
            {
                var entity = _mapper.Map<WarnTypeModel>(req);
                entity.Create = account;
                await _warnType.AddAsync(entity);
                _log.LogInformation($"{account}添加标识为{entity.Id}的报警类型成功");
                return new HandleResponse<int> { Key = entity.Id, Success = true, Message = "添加数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加报警类型失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> ModifyWarnTypeAsync(string account, WarnTypeUpdateDto req)
        {
            //验证是否重名
            var type = await _warnType.Find(a => a.TypeName == req.TypeName && a.Id != req.Id).FirstOrDefaultAsync();
            if (type != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同的类型名称，请确认" };
            }
            try
            {
                var wt = await _warnType.FindAsync(req.Id);
                if (wt == null)
                {
                    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
                }
                var entity = _mapper.Map(req, wt);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                _log.LogInformation($"{account}修改标识为{req.Id}的报警类型成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改报警类型失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> DeleteWarnTypeAsync(string account, int Id)
        {
            var type = await _warnType.FindWithCodeAsync(a => a.Id == Id);
            if (type == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            }
            else
            {
                if (type.WarnCode.Count > 0)
                {
                    return new BaseResponse { Success = false, Message = "类型下存在相关数据，不能删除" };
                }
            }
            try
            {
                await _warnType.RemoveAsync(type);
                _log.LogInformation($"{account}删除标识为{Id}的报警类型成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标识为{Id}的报警类型失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> FindWarnTypeByIdAsync(int Id)
        {
            var wt = await _warnType.FindAsync(Id);
            if (wt == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            }
            var dto = _mapper.Map<WarnTypeDto>(wt);
            return new BResponse<WarnTypeDto> { Success = true, Message = "获取数据成功", Data = dto };
        }

        public async Task<BaseResponse> FindWarnTypeAsync()
        {
            var wt = await _warnType.Find(a => true).ToListAsync();
            var dtos = _mapper.Map<List<WarnTypeDto>>(wt);
            return new BResponse<List<WarnTypeDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
    }
}
