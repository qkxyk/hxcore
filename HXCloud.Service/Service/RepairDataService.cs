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
    /// <summary>
    /// 维修工单流程数据
    /// </summary>
    public class RepairDataService : IRepairDataService
    {
        private readonly ILogger<RepairDataService> _logger;
        private readonly IRepairDataRepository _repairData;
        private readonly IRepairRepository _repair;
        private readonly IMapper _mapper;

        public RepairDataService(ILogger<RepairDataService> logger, IRepairDataRepository repairData, IRepairRepository repair, IMapper mapper)
        {
            this._logger = logger;
            this._repairData = repairData;
            this._repair = repair;
            this._mapper = mapper;
        }
        public Task<bool> IsExist(Expression<Func<RepairDataModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 查找工单最后一个流程的流程编号自动加1
        /// </summary>
        /// <param name="repairId"></param>
        /// <returns></returns>
        private async Task<int> GetLastRepairDataSnAsync(string repairId)
        {
            var data = await _repairData.Find(a => a.RepairId == repairId).OrderByDescending(a => a.Sn).Take(1).FirstOrDefaultAsync();
            if (data == null)
            {
                return 1;
            }
            else
            {
                return data.Sn + 1;
            }
        }
        /// <summary>
        /// 添加工单流程数据
        /// </summary>
        /// <param name="Operate">操作人</param>
        /// <param name="account">操作人账号</param>
        /// <param name="req">工单流程数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddRepairDataAsync(string Operate, string account, AddRepairDataBaseDto req)
        {
            //检测工单是否合法
            var rep = _repair.Find(req.RepairId);
            if (rep == null)
            {
                return new BaseResponse { Success = false, Message = "输入的工单不存在，请确认" };
            }

            int sn = await GetLastRepairDataSnAsync(req.RepairId);
            try
            {
                var entity = _mapper.Map<RepairDataModel>(req);
                entity.Operator = Operate;
                entity.OperDate = DateTime.Now;
                entity.Sn = sn;
                await _repairData.SaveAsync(entity);
                _logger.LogInformation($"{Operate}添加设置编号为{entity.Id}的运维流程单成功");
                return new BaseResponse { Success = true, Message = "设置成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Operate}添加运维流程单失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"用户添加运维流程单失败" };
            }
        }
        /// <summary>
        /// 接单
        /// </summary>
        /// <param name="Operate">操作人姓名</param>
        /// <param name="account">操作人账号</param>
        /// <param name="req">单号信息</param>
        /// <returns></returns>
        public async Task<BaseResponse> ReceiveAsync(string Operate, string account, AddRepairDataBaseDto req)
        {
            int sn = await GetLastRepairDataSnAsync(req.RepairId);
            try
            {
                var entity = _mapper.Map<RepairDataModel>(req);
                entity.Operator = Operate;
                entity.OperDate = DateTime.Now;
                entity.Sn = sn;
                entity.RepairStatus = RepairStatus.Way;
                entity.Id = Guid.NewGuid().ToString("N");
                await _repairData.AddAsync(entity, RepairStatus.Way);
                _logger.LogInformation($"{Operate}接单编号为{entity.Id}的运维流程单成功");
                return new BaseResponse { Success = true, Message = "接单成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Operate}用户接单失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"用户接单失败" };
            }
        }
        /// <summary>
        /// 设置运维单第三方维修
        /// </summary>
        /// <param name="Operate">操作人姓名</param>
        /// <param name="account">操作人账号</param>
        /// <param name="req">设置数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> ThirdPartAsync(string Operate, string account, AddRepairDataMessageDto req)
        {
            int sn = await GetLastRepairDataSnAsync(req.RepairId);
            try
            {
                var entity = _mapper.Map<RepairDataModel>(req);
                entity.RepairStatus = RepairStatus.Third;
                entity.Operator = Operate;
                entity.OperDate = DateTime.Now;
                entity.Sn = sn;
                entity.Id = Guid.NewGuid().ToString("N");
                await _repairData.AddAsync(entity, RepairStatus.Third);
                _logger.LogInformation($"{Operate}设置编号为{entity.Id}的运维流程单成功");
                return new BaseResponse { Success = true, Message = "设置成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Operate}用户接单失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"设置失败" };
            }
        }

        /// <summary>
        /// 设置运维单等待配件
        /// </summary>
        /// <param name="Operate">操作人姓名</param>
        /// <param name="account">操作人账号</param>
        /// <param name="req">设置数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> WaitAsync(string Operate, string account, AddRepairDataMessageDto req)
        {
            int sn = await GetLastRepairDataSnAsync(req.RepairId);
            try
            {
                var entity = _mapper.Map<RepairDataModel>(req);
                entity.Operator = Operate;
                entity.OperDate = DateTime.Now;
                entity.Sn = sn;
                entity.RepairStatus = RepairStatus.Wait;
                entity.Id = Guid.NewGuid().ToString("N");
                await _repairData.AddAsync(entity, RepairStatus.Wait);
                _logger.LogInformation($"{Operate}设置编号为{entity.Id}的运维流程单成功");
                return new BaseResponse { Success = true, Message = "设置成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Operate}用户接单失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"设置失败" };
            }
        }

        /// <summary>
        /// 上传运维凭证数据
        /// </summary>
        /// <param name="Operate">操作人姓名</param>
        /// <param name="account">操作人账号</param>
        /// <param name="req">凭证数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> UploadAsync(string Operate, string account, RepairSubmitDto req)
        {
            int sn = await GetLastRepairDataSnAsync(req.RepairId);
            try
            {
                var entity = _mapper.Map<RepairDataModel>(req);
                entity.Operator = Operate;
                entity.OperDate = DateTime.Now;
                entity.Sn = sn;
                entity.Id = Guid.NewGuid().ToString("N");
                entity.RepairStatus = RepairStatus.Check;
                await _repairData.AddUploadAsync(entity, RepairStatus.Check,req.FaultCode);
                ////更新维修的code
                //if (!string.IsNullOrEmpty(req.FaultCode))
                //{
                //    var repairEntity = await _repair.FindAsync(req.RepairId);
                //    repairEntity.FaultCode = req.FaultCode;
                //    repairEntity.Modify = account;
                //    repairEntity.ModifyTime = DateTime.Now;
                //    await _repair.SaveAsync(repairEntity);
                //}
                _logger.LogInformation($"{Operate}设置编号为{entity.Id}的运维流程单成功");
                return new BaseResponse { Success = true, Message = "设置成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Operate}用户接单失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"设置失败" };
            }
        }
        /// <summary>
        /// 审核运维流程单
        /// </summary>
        /// <param name="Operate">操作人姓名</param>
        /// <param name="account">操作人账号</param>
        /// <param name="complete">流程是否结束</param>
        /// <param name="req">审核数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> CheckAsync(string Operate, string account, bool complete,AddRepairCheckDto req)
        {
            int sn = await GetLastRepairDataSnAsync(req.RepairId);
            try
            {
                var entity = _mapper.Map<RepairDataModel>(req);
                entity.Operator = Operate;
                entity.OperDate = DateTime.Now;
                entity.Sn = sn;
                entity.Id = Guid.NewGuid().ToString("N");
                RepairStatus status = RepairStatus.Way;
                if (complete)
                {
                    status = RepairStatus.Complete;
                }
                entity.RepairStatus = status;
                await _repairData.AddAsync(entity, status);
                _logger.LogInformation($"{Operate}设置编号为{entity.Id}的运维流程单成功");
                return new BaseResponse { Success = true, Message = "设置成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Operate}用户接单失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"设置失败" };
            }
        }
    }
}
