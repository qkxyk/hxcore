using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly ILogger<ModuleService> _log;
        private readonly IMapper _mapper;

        public ModuleService(IModuleRepository moduleRepository, ILogger<ModuleService> log, IMapper mapper)
        {
            this._moduleRepository = moduleRepository;
            this._log = log;
            this._mapper = mapper;
        }
        public Task<bool> IsExist(Expression<Func<ModuleModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 用于httppath部分修改数据
        /// </summary>
        /// <param name="Id">模块标识</param>
        /// <returns></returns>
        public async Task<ModuleDto> GetModuleByIdAsync(int Id)
        {
            var data = await _moduleRepository.FindAsync(Id);
            var dto = _mapper.Map<ModuleDto>(data);
            return dto;
        }
        public async Task<BaseResponse> GetModuleDataByIdAsync(int Id)
        {
            var data = await _moduleRepository.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的模块编号不存在" };
            }
            var dto = _mapper.Map<ModuleDto>(data);
            return new BResponse<ModuleDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetModulesAsync()
        {
            var data = await _moduleRepository.Find(a => 1 == 1).ToListAsync();
            var dtos = _mapper.Map<List<ModuleDto>>(data);
            return new BResponse<List<ModuleDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
        /// <summary>
        /// 添加系统模块
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">模块参数</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddModuleAsync(string Account, ModuleAddDto req)
        {
            var data = await _moduleRepository.Find(a => a.ModuleName == req.ModuleName || a.Code == req.Code).CountAsync();
            if (data > 0)
            {
                return new BaseResponse { Success = false, Message = "输入的模块名称或者模块编号已存在" };
            }
            try
            {
                var entity = _mapper.Map<ModuleModel>(req);
                entity.Create = Account;
                await _moduleRepository.AddAsync(entity);
                _log.LogInformation($"{Account}添加标识为{entity.Id}的模块成功");
                return new BResponse<int> { Success = true, Message = "添加数据成功", Data = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加模块失败，失败原因{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "添加模块失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateModuleAsync(string Account, int Id, ModuleAddDto req)
        {
            var data = await _moduleRepository.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "模块不存在" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = Account;
                entity.ModifyTime = DateTime.Now;
                await _moduleRepository.SaveAsync(entity);
                _log.LogInformation($"{Account}修改标识为{Id}的模块成功");
                return new BResponse<int> { Success = true, Message = "修改数据成功", Data = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改模块数据失败，失败原因{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "修改模块数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteModuleAsync(string Account, int Id)
        {
            var data = await _moduleRepository.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "模块不存在" };
            }
            try
            {
                await _moduleRepository.RemoveAsync(data);
                _log.LogInformation($"{Account}删除标识为{Id}的模块成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}删除模块数据失败，失败原因{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "删除模块数据失败，请联系管理员" };
            }
        }

        /// <summary>
        /// 修改模块数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">修改数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> PatchModuleAsync(string Account, ModuleDto req)
        {
            try
            {
                var data = await _moduleRepository.FindAsync(req.Id);
                _mapper.Map(req, data);
                data.Modify = Account;
                data.ModifyTime = DateTime.Now;
                await _moduleRepository.SaveAsync(data);
                _log.LogInformation($"{Account}修改标识为{req.Id}的模块成功");
                return new BResponse<int> { Success = true, Message = "修改数据成功", Data = req.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改模块数据失败，失败原因{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "修改模块数据失败，请联系管理员" };
            }
        
        }
        /// <summary>
        /// 根据模块Code获取模块编号
        /// </summary>
        /// <param name="Code">模块编码</param>
        /// <returns></returns>
        public async Task<int> GetModuleIdByCodeAsync(string Code)
        {
            var Id = await _moduleRepository.Find(a => a.Code == Code).Select(a => a.Id).FirstOrDefaultAsync();
            return Id;
        }
    }
}
