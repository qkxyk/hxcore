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
    public class RepairPartService : IRepairPartService
    {
        private readonly ILogger<RepairPartService> _logger;
        private readonly IMapper _map;
        private readonly IRepairRepository _repair;
        private readonly IRepairPartRepository _part;

        public RepairPartService(ILogger<RepairPartService> log,IMapper map,IRepairRepository repair,IRepairPartRepository part)
        {
            this._logger = log;
            this._map = map;
            this._repair = repair;
            this._part = part;
        }
        public Task<bool> IsExist(Expression<Func<RepairPartModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 添加运维配件数据，因为没法校验配件数据，可能会造成重复添加的问题，故假定一个运维单只能添加5条配件数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Operate">操作人姓名</param>
        /// <param name="req">运维配件数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddRepairPartAsync(string Account, string Operate, RepairPartAddDto req)
        {
            var rep =  _repair.Find(req.RepairId);
            if (rep==null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单编号不存在" };
            }
            var count = await _part.Find(a => a.RepairId == req.RepairId).CountAsync();
            if (count>=5)
            {
                return new BaseResponse { Success = false, Message = "该运维单申请太多配件，请确认" };
            }
            try
            {
                var entity = _map.Map<RepairPartModel>(req);
                entity.Operate = Operate;
                entity.OperateTime = DateTime.Now;
                await _part.AddAsync(entity);
                _logger.LogInformation($"{Account}添加标识为{entity.Id}的运维配件成功");
                return new BaseResponse { Success = true, Message = "添加运维配件成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Account}添加{req.RepairId}的运维单配件失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"添加运维配件失败" };
            }
        }
    }
}
