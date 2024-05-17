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
        private readonly IModuleRepository _moduleRepository;

        public ModuleOperateService(ILogger<ModuleOperateService> log, IMapper mapper, IModuleOperateRepository moduleOperateRepository,IModuleRepository moduleRepository)
        {
            this._log = log;
            this._mapper = mapper;
            this._moduleOperateRepository = moduleOperateRepository;
            this._moduleRepository = moduleRepository;
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
        public async Task<BaseResponse> AddModuleOperateAsync(string Account, int ModuleId, ModuleOperateAddDto req)
        {
            //获取模块所有的操作
            var operates = await _moduleOperateRepository.Find(a => a.ModuleId == ModuleId).ToListAsync();
            if (operates!=null)
            {
                //验证同一个组内是否存在相同编号
                var codes = operates.Find(a => a.SerialNumber == req.SerialNumber && a.Code == req.Code);
                if (codes!=null)
                {
                    return new BaseResponse { Success = false, Message = "该模块已添加过相同的操作" };
                }
                //验证输入的编号和编号名称是否和存在的编号名称相同
                var serialName = operates.Find(a => a.SerialNumber == req.SerialNumber && a.SerialName != req.SerialName);
                if (serialName!=null)
                {
                    return new BaseResponse { Success = false, Message = "相同的编号输入的编号名称不同" };
                }
            }
            //var data = await _moduleOperateRepository.Find(a => a.ModuleId == ModuleId &&(a.Code==req.Code|| a.OperateName == req.OperateName)).CountAsync();
            //if (data > 0)
            //{
            //    return new BaseResponse { Success = false, Message = "该模块已添加过该操作，请勿重复添加" };
            //}
            try
            {
                var entity = _mapper.Map<ModuleOperateModel>(req);
                entity.ModuleId = ModuleId;
                
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

        /// <summary>
        /// 根据模块码和操作码获取模块操作标识
        /// </summary>
        /// <param name="ModuleCode">模块码</param>
        /// <param name="Code">操作码</param>
        /// <returns></returns>
        public async Task<int> GetModuleOperateIdByModuleCodeAndOperateCodeAsync(string ModuleCode,string Code)
        {
            var data = await _moduleOperateRepository.Find(a => a.Module.Code == ModuleCode && a.Code == Code).FirstOrDefaultAsync();
            if (data==null)
            {
                return 0;
            }
            return data.Id;
        }
    }
}
