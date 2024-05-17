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
    public class OpsFaultTypeService : IOpsFaultTypeService
    {
        private readonly IMapper _map;
        private readonly ILogger<OpsFaultTypeService> _log;
        private readonly IOpsFaultTypeRepository _opsFaultType;
        private readonly IOpsFaultRepository _opsFault;

        public OpsFaultTypeService(IMapper map, ILogger<OpsFaultTypeService> log, IOpsFaultTypeRepository opsFaultType, IOpsFaultRepository opsFault)
        {
            this._map = map;
            this._log = log;
            this._opsFaultType = opsFaultType;
            this._opsFault = opsFault;
        }
        public async Task<bool> IsExist(Expression<Func<OpsFaultTypeModel, bool>> predicate)
        {
            var data = await _opsFaultType.Find(predicate).FirstOrDefaultAsync();
            if (data != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证运维故障类型数据是否存在，不存在返回-1,存在则返回该类型子节点或者顶级节点
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> IsExistAsync(Expression<Func<OpsFaultTypeModel, bool>> predicate)
        {
            var data = await _opsFaultType.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return -1;
            }
            return data.Flag;
        }
        /// <summary>
        /// 添加故障类型,故障类型只能添加两层
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">故障类型数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddOpsFaultTypeAsync(string account, OpsFaultTypeAddDto req)
        {
            //故障类型只能添加两层
            if (req.ParentId.HasValue)
            {
                if (req.ParentId.Value == 0)
                {
                    req.Flag = 0;
                }
                else
                {
                    req.Flag = 1;
                    var data = await _opsFaultType.FindAsync(req.ParentId.Value);
                    if (data == null)
                    {
                        return new BaseResponse { Success = false, Message = "输入的父类型不存在" };
                    }
                    if (data.ParentId.HasValue && data.ParentId.Value != 0)
                    {
                        return new BaseResponse { Success = false, Message = "故障类型子节点不能再添加节点" };
                    }
                }
            }
            else
            {
                req.Flag = 0;
            }
            try
            {
                var entity = _map.Map<OpsFaultTypeModel>(req);
                entity.Create = account;
                await _opsFaultType.AddAsync(entity);
                _log.LogInformation($"{account}添加标识为{entity.FaultTypeId}的故障类型成功");
                return new HandleIdResponse<int> { Id = entity.FaultTypeId, Success = true, Message = "添加数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加故障类型失败，失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "添加故障类型失败" };
            }
        }
        /// <summary>
        /// 修改故障类型数据，只能修改故障类型名称
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<BaseResponse> EditOpsFaultTypeAsync(string account, OpsFaultTypeEditDto req)
        {
            try
            {
                var entity = await _opsFaultType.FindAsync(req.Id);
                if (entity.FaultTypeId != req.Id && entity.FaultTypeName == req.FaultTypeName)
                {
                    return new BaseResponse { Success = false, Message = "已存在相同名称的故障类型" };
                }
                entity.FaultTypeName = req.FaultTypeName;
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _opsFaultType.SaveAsync(entity);
                _log.LogInformation($"{account}修改标识为{req.Id}的故障类型数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标识为{req.Id}的故障类型数据失败，失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "修改数据失败" };
            }
        }
        /// <summary>
        /// 删除故障类型
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">故障类型编号</param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteOpsFaultTypeAsync(string account, int Id)
        {
            try
            {
                var fault = await _opsFault.Find(a => a.OpsFaultTypeId == Id).FirstOrDefaultAsync();
                if (fault != null)
                {
                    return new BaseResponse { Success = false, Message = "该故障类型存在故障数据，请先删除该故障类型的故障数据" };
                }
                var entity = await _opsFaultType.FindAsync(Id);
                await _opsFaultType.RemoveAsync(entity);
                _log.LogInformation($"{account}删除标识为{Id}的故障类型数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标识为{Id}的故障类型数据失败，失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "删除数据失败" };
            }
        }
        /// <summary>
        /// 根据故障类型Id获取故障类型，包含该故障类型关联的所有故障数据，包含验证故障类型是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Flag">是否顶级节点</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetOpsFaultTypeByIdAsync(int Id, int Flag = 0)
        {
            OpsFaultTypeModel data = null;
            if (Flag == 0)//顶级节点
            {
                data = await _opsFaultType.FindWithOpsFaultAsync(Id);
                var dto = _map.Map<OpsFaultTypeParentDto>(data);
                return new BResponse<OpsFaultTypeParentDto> { Success = true, Message = "获取数据成功", Data = dto };
            }
            else
            {
                data = await _opsFaultType.FindWithOpsFaultChildAsync(Id);
                var dto = _map.Map<OpsFaultTypeDto>(data);
                return new BResponse<OpsFaultTypeDto> { Success = true, Message = "获取数据成功", Data = dto };
            }
        }
        //public async Task<BaseResponse> GetOpsFaultTypeByIdChildAsync(int )

        /// <summary>
        /// 获取全部故障类型数据
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse> GetAllOpsFaultTypeAsync()
        {
            var data = await _opsFaultType.FindAllWithOpsFaultAsync();
            var dto = _map.Map<List<OpsFaultTypeParentDto>>(data);
            return new BResponse<List<OpsFaultTypeParentDto>> { Success = true, Message = "获取数据成功", Data = dto };
        }
        /// <summary>
        /// 根据父标识获取全部故障类型
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetOpsFaultTypeByParentIdAsync(int Id)
        {
            var data = await _opsFaultType.FindByParentIdWithOpsFaultAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的故障类型不存在" };
            }
            var dto = _map.Map<OpsFaultTypeDto>(data);
            return new BResponse<OpsFaultTypeDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
    }
}
