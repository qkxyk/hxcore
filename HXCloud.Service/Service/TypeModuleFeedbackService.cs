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
    public class TypeModuleFeedbackService : ITypeModuleFeedbackService
    {
        private readonly ILogger<TypeModuleFeedbackService> _log;
        private readonly IMapper _map;
        private readonly ITypeModuleFeedbackRepository _tmfr;

        public TypeModuleFeedbackService(ILogger<TypeModuleFeedbackService> log, IMapper map, ITypeModuleFeedbackRepository tmfr)
        {
            this._log = log;
            this._map = map;
            this._tmfr = tmfr;
        }

        public Task<bool> IsExist(Expression<Func<TypeModuleFeedbackModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> AddTypeModuleFeedbaskAsync(string Account, TypeModuleFeedbackAddDto req)
        {
            var count = await _tmfr.Find(a => a.ModuleControlId == req.ModuleControlId && a.DataDefineId == req.DataDefineId).CountAsync();
            if (count > 0)
            {
                return new BaseResponse { Success = false, Message = "已添加过相同的数据" };
            }
            try
            {
                var entity = _map.Map<TypeModuleFeedbackModel>(req);
                entity.Create = Account;
                await _tmfr.AddAsync(entity);
                _log.LogInformation($"{Account}添加标示为{entity.Id}的反馈数据成功");
                return new HandleResponse<int> { Success = true, Message = "添加数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加标示为{req.ModuleControlId}的控制项的反馈数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateTypeModuleFeedbackAsync(string Account, TypeModuleFeedbackUpdateDto req)
        {
            var data = await _tmfr.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据不存在" };
            }
            var count = await _tmfr.Find(a => a.ModuleControlId == req.ModuleControlId && a.DataDefineId == req.DataDefineId && a.Id != req.Id).CountAsync();
            if (count > 0)
            {
                return new BaseResponse { Success = false, Message = "该控制项下已存在相同的反馈数据" };
            }
            try
            {
                var entity = _map.Map(req, data);
                entity.Modify = Account;
                entity.ModifyTime = DateTime.Now;
                await _tmfr.SaveAsync(entity);
                _log.LogInformation($"{Account}修改标示为{req.Id}的反馈数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改标示为{req.Id}的反馈项失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteTypeModuleFeedbackAsync(string Account, int Id)
        {
            var data = await _tmfr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据不存在" };
            }
            try
            {
                await _tmfr.RemoveAsync(data);
                _log.LogInformation($"{Account}删除标示为{Id}的反馈数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}删除标示为{Id}的反馈数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetFeedbackByControlIdAsync(int ControlId)
        {
            var data = await _tmfr.FindWithDataDefineAsync(a => a.ModuleControlId == ControlId);
            var dtos = _map.Map<IEnumerable<TypeModuleFeedbackDto>>(data);
            return new BResponse<IEnumerable<TypeModuleFeedbackDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
    }
}
