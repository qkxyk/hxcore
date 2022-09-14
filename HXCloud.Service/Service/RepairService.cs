﻿using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class RepairService : IRepairService
    {
        private readonly ILogger<RepairService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepairRepository _repair;

        public RepairService(ILogger<RepairService> logger, IMapper mapper, IRepairRepository repair)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._repair = repair;
        }
        public Task<bool> IsExist(Expression<Func<RepairModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 查找维修信息
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public async Task<RepairModel> IsExistAsync(Expression<Func<RepairModel, bool>> predicate)
        {
            var data = await _repair.Find(predicate).FirstOrDefaultAsync();
            return data;
        }

        /// <summary>
        /// 下发维修或者调试单
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">操作人标识</param>
        /// <param name="req">维修或者操作信息</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddRepairAsync(string account, int Id, RepairAddDto req)
        {
            string repairType = req.RepairType == 0 ? "维修" : "调试";
            try
            {
                var RepairlId = DateTime.Now.ToString("yyyyMMddHHmmss");
                RepairlId += Id.ToString();
                var entity = _mapper.Map<RepairModel>(req);
                entity.Id = RepairlId;
                entity.Create = account;
                await _repair.AddAsync(entity);

                _logger.LogInformation($"{account}创建标识为{entity.Id}的{repairType}单成功");
                return new HandleResponse<string>() { Key = entity.Id, Success = true, Message = $"创建{repairType}单成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}创建{repairType}单失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"用户创建{repairType}单失败" };
            }
        }
        /// <summary>
        /// 运维单接单
        /// </summary>
        /// <param name="Id">运维单编号</param>
        /// <param name="Account">接单人</param>
        /// <param name="status">运维状态</param>
        /// <returns></returns>
        public async Task<BaseResponse> ReceiveRepairAsync(string Id, string Account, int status)
        {
            try
            {
                var data = await _repair.FindAsync(Id);
                data.RepairStatus = RepairStatus.Way;
                if (status == 1)
                {
                    data.Receiver = Account;
                    data.ReceiveTime = DateTime.Now;
                }
                else
                {
                    data.Modify = Account;
                    data.ModifyTime = DateTime.Now;
                }
                await _repair.SaveAsync(data);
                _logger.LogInformation($"{Account}接收编号为{Id}的运维单成功");
                return new BaseResponse { Success = true, Message = "接单成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Account}接收编号为{Id}的运维失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"用户接单失败" };
            }
        }
        /// 设置运维单为等待配件状态
        /// </summary>
        /// <param name="Id">运维单编号</param>
        /// <param name="Account">接单人</param>
        /// <param name="status">运维状态</param>
        /// <returns></returns>
        public async Task<BaseResponse> WaitRepairAsync(string Id, string Account, int status)
        {
            try
            {
                var data = await _repair.FindAsync(Id);
                data.RepairStatus = RepairStatus.Way;
                if (status == 2)
                {
                    //data.Receiver = Account;
                    data.WaitTime = DateTime.Now;
                }
                else
                {
                    data.Modify = Account;
                    data.ModifyTime = DateTime.Now;
                }
                await _repair.SaveAsync(data);
                _logger.LogInformation($"{Account}设置编号为{Id}的运维单成功");
                return new BaseResponse { Success = true, Message = "设置成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Account}设置编号为{Id}的运维失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"用户设置失败" };
            }
        }

        /// <summary>
        /// 上传调试或者维修数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">调试或者维修数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> UploadRepairImageAsync(string account, RepairAddImageDto req)
        {
            string repairType = "";
            try
            {
                var entity = await _repair.FindAsync(req.Id);
                if (entity.RepairType == RepairType.Repair)
                {
                    repairType = "维修";
                }
                else
                {
                    repairType = "调试";
                }
                entity.RepairStatus = RepairStatus.Check;
                entity.Description = req.Description;
                entity.CheckTime = DateTime.Now;
                entity.Url = req.Url;
                await _repair.SaveAsync(entity);
                _logger.LogInformation($"{account}上传标示为{entity.Id}的{repairType}数据成功");
                return new BaseResponse { Success = true, Message = $"上传{repairType}数据成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}上传标识为{req.Id}的{repairType}数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"上传{repairType}数据失败" };
            }
        }
        /// <summary>
        /// 审核运维单
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">审核数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> CheckRepairAsync(string account, RepairCheckDto req)
        {
            string repairType = "";
            try
            {
                var entity = await _repair.FindAsync(req.Id);
                if (entity.RepairType == RepairType.Repair)
                {
                    repairType = "维修";
                }
                else
                {
                    repairType = "调试";
                }
                if (req.Check)
                {
                    entity.RepairStatus = RepairStatus.Complete;
                    entity.CompleteTime = DateTime.Now;
                }
                else
                {
                    entity.RepairStatus = RepairStatus.Way;//回退状态
                    entity.ModifyTime = DateTime.Now;
                }

                entity.CheckDescription = req.Description;
                entity.Modify = account;

                //entity.CheckTime = DateTime.Now;
                await _repair.SaveAsync(entity);
                _logger.LogInformation($"{account}核验标示为{entity.Id}的{repairType}数据成功");
                return new BaseResponse { Success = true, Message = $"核验{repairType}数据成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}核验标识为{req.Id}的{repairType}数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"核验{repairType}数据失败" };
            }
        }
        /// <summary>
        /// 删除运维单，只能删除状态为未接单的
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">运维单编号</param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteRepairAsync(string account, string Id)
        {
            string rt = "";
            try
            {
                var entity = await _repair.FindAsync(Id);
                if (entity == null)
                {
                    return new BaseResponse { Success = false, Message = "输入的运维单不存在" };
                }
                if (entity.RepairType == RepairType.Repair)
                {
                    rt = "维修";
                }
                else
                {
                    rt = "调试";
                }
                if (entity.RepairStatus != RepairStatus.Send)
                {
                    return new BaseResponse { Success = false, Message = $"{rt}已被接单，不能删除" };
                }
                await _repair.RemoveAsync(entity);
                _logger.LogInformation($"{account}删除标识为{Id}的{rt}单成功");
                return new BaseResponse { Success = true, Message = $"删除{rt}单成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}删除标识为{Id}的{rt}单失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"删除{rt}单失败,请联系管理员" };
            }
        }

        /// <summary>
        /// 获取用户的运维单
        /// </summary>
        /// <param name="req">运维单状态和类型</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetRepairAsync(RepairRequest req)
        {
            var data = _repair.Find(a => a.Receiver == req.Account);
            data = data.Where(a => a.RepairType == (RepairType)req.RepairType);
            data = data.Where(a => a.RepairStatus == (RepairStatus)req.RepairStatus);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => a.DeviceName.Contains(req.Search));
            }
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "CreateTime Desc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var list = await data.OrderBy(OrderExpression).ToListAsync();
            var dtos = _mapper.Map<List<RepairDto>>(list);
            return new BResponse<List<RepairDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
        /// <summary>
        /// 获取用户的分页运维单
        /// </summary>
        /// <param name="req">运维单状态和类型</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetPageRepairAsync(RepairPageRequest req)
        {
            var data = _repair.Find(a => a.Receiver == req.Account);
            data = data.Where(a => a.RepairType == (RepairType)req.RepairType);
            data = data.Where(a => a.RepairStatus == (RepairStatus)req.RepairStatus);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => a.DeviceName.Contains(req.Search));
            }
            int count = data.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "CreateTime Desc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var list = await data.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<RepairDto>>(list);
            var ret = new BasePageResponse<List<RepairDto>>();
            ret.Success = true;
            ret.Message = "获取数据成功";
            ret.PageSize = req.PageSize;
            ret.CurrentPage = req.PageNo;
            ret.Count = count;
            ret.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            ret.Data = dtos;
            return ret;
        }
    }
}
