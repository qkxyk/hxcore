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
    public class ModuleOperateService : IModuleOperateService
    {
        private readonly ILogger<ModuleOperateService> _log;
        private readonly IMapper _mapper;
        private readonly IModuleOperateRepository _moduleOperateRepository;

        public ModuleOperateService(ILogger<ModuleOperateService> log, IMapper mapper, IModuleOperateRepository moduleOperateRepository)
        {
            this._log = log;
            this._mapper = mapper;
            this._moduleOperateRepository = moduleOperateRepository;
        }
        public async Task<bool> IsExist(Expression<Func<ModuleOperateModel, bool>> predicate)
        {
            var data = await _moduleOperateRepository.Find(predicate).CountAsync();
            if (data > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加模块操作
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">模块操作数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddModuleOperateAsync(string Account, ModuleOperateAddDto req)
        {
            var data = await _moduleOperateRepository.Find(a => a.ModuleId == req.ModuleId && a.OperateName == req.OperateName).CountAsync();
            if (data > 0)
            {
                return new BaseResponse { Success = false, Message = "该模块已添加过该操作，请勿重复添加" };
            }
            try
            {
                var entity = _mapper.Map<ModuleOperateModel>(req);
                await _moduleOperateRepository.AddAsync(entity);
                _log.LogInformation($"{Account}添加标识为{entity.Id}的模块操作成功");
                return new BResponse<int> { Success = true, Message = "添加数据成功", Data = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加模块操作失败，失败原因：{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 根据模块操作标识获取模块操作数据
        /// </summary>
        /// <param name="Id">模块操作标识</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetModuleOperateByIdAsync(int Id)
        {
            var data = await _moduleOperateRepository.FindAsync(Id);
            var dto = _mapper.Map<ModuleOperateDto>(data);
            return new BResponse<ModuleOperateDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        /// <summary>
        /// 获取模块操作
        /// </summary>
        /// <param name="ModuleId">模块标识</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetModuleOperatesAsync(int ModuleId)
        {
            var data = await _moduleOperateRepository.Find(a => a.ModuleId == ModuleId).ToListAsync();
            var dtos = _mapper.Map<List<ModuleOperateDto>>(data);
            return new BResponse<List<ModuleOperateDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
        /// <summary>
        /// 删除模块操作数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="Id">模块操作标识</param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteModuleOperateByIdAsync(string Account, int Id)
        {
            var data = await _moduleOperateRepository.FindAsync(Id);
            try
            {
                await _moduleOperateRepository.RemoveAsync(data);
                _log.LogInformation($"{Account}删除标识为{Id}的模块操作成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}删除标识为{Id}的模块操作失败，失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
    }
}
