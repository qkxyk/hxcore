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
    public class RoleModuleOperateService : IRoleModuleOperateService
    {
        private readonly ILogger<RoleModuleOperateService> _logger;
        private readonly IMapper _mapper;
        private readonly IRoleModuleOperateRepository _roleModuleOperate;
        private readonly IModuleOperateRepository _moduleOperateRepository;
        private readonly IRoleRepository _roleRepository;

        public Task<bool> IsExist(Expression<Func<RoleModuleOperateModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public RoleModuleOperateService(ILogger<RoleModuleOperateService> logger, IMapper mapper, IRoleModuleOperateRepository roleModuleOperate,
            IModuleOperateRepository moduleOperateRepository,IRoleRepository roleRepository)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._roleModuleOperate = roleModuleOperate;
            this._moduleOperateRepository = moduleOperateRepository;
            this._roleRepository = roleRepository;
        }
        /// <summary>
        /// 添加角色模块操作
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<BaseResponse> AddRoleModuleOperateAsync(string account, RoleModuleOperateAddDto req)
        {
            //检测操作是否存在
            var operate = await _moduleOperateRepository.FindAsync(req.OperateId);
            if (operate == null)
            {
                return new BaseResponse { Success = false, Message = "输入的模块操作不存在" };
            }
            //检测角色是否数据该模块
            var role = await _roleRepository.Find(a => a.Id == req.RoleId && a.ModuleId == operate.ModuleId).CountAsync();
            if (role<=0)
            {
                return new BaseResponse { Success = false, Message = "输入的角色不存在或者角色不能分配该模块操作" };
            }
            //检测是否重复添加
            var data = await _roleModuleOperate.Find(a => a.OperateId == req.OperateId && a.RoleId == req.RoleId).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "已添加过相同的数据" };
            }
            try
            {
                await _roleModuleOperate.AddAsync(new RoleModuleOperateModel { RoleId = req.RoleId, OperateId = req.OperateId, Create = account });
                _logger.LogInformation($"{account}添加角色标识为:{req.RoleId},操作标识为:{req.OperateId}的模块操作成功");
                return new BaseResponse { Success = true, Message = "添加数据成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}添加角色标识为:{req.RoleId},操作标识为:{req.OperateId}的模块操作失败,失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
                throw;
            }
        }
        /// <summary>
        /// 删除角色模块操作
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteRoleModuleOperateAsync(string account, int RoleId,int OperateId)
        {
            var data = await _roleModuleOperate.Find(a => a.OperateId == OperateId && a.RoleId == RoleId).FirstOrDefaultAsync();
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "要删除的数据不存在" };
            }
            try
            {
                await _roleModuleOperate.RemoveAsync(data);
                _logger.LogInformation($"{account}删除角色标识为:{RoleId},操作标识为:{OperateId}的模块操作成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}删除角色标识为:{RoleId},操作标识为:{OperateId}的模块操作失败,失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
                throw;
            }
        }
        /// <summary>
        /// 获取角色分配的操作
        /// </summary>
        /// <param name="RoleId">角色标识</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetRoleOperatesAsync(int RoleId)
        {
            var role = await _roleRepository.FindAsync(RoleId);
            if (role==null)
            {
                return new BaseResponse { Success = false, Message = "输入的角色标识不存在" };
            }
            var data = await _roleModuleOperate.Find(a => a.RoleId == RoleId).ToListAsync();
            var ret = _mapper.Map<List<RoleModuleOperateDto>>(data);
            return new BResponse<List<RoleModuleOperateDto>> { Success = true, Message = "获取数据成功", Data = ret };
        }
        /// <summary>
        /// 根据操作标识获取分配的角色列表
        /// </summary>
        /// <param name="OperateId">操作标识</param>
        /// <returns></returns>
        public async Task<List<int> > GetModuleOperatesAsync(int OperateId)
        {
            var data = await _roleModuleOperate.Find(a => a.OperateId == OperateId).Select(a => a.RoleId).ToListAsync();
            return data;
        }
    }
}
