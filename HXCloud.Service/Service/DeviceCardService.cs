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
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace HXCloud.Service
{
    public class DeviceCardService : IDeviceCardService
    {
        private readonly ILogger<DeviceCardService> _log;
        private readonly IMapper _mapper;
        private readonly IDeviceCardRepository _dcr;

        public DeviceCardService(ILogger<DeviceCardService> log, IMapper mapper, IDeviceCardRepository dcr)
        {
            this._log = log;
            this._mapper = mapper;
            this._dcr = dcr;
        }
        public async Task<bool> IsExist(Expression<Func<DeviceCardModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> AddDeviceCardAsync(string DeviceSn, DeviceCardAddDto req, string account)
        {
            //检查该设备是否已存在流量卡
            var d = await _dcr.Find(a => a.DeviceSn == DeviceSn).CountAsync();
            if (d > 0)
            {
                return new BaseResponse { Success = false, Message = "该设备已存在流量卡数据" };
            }
            if (req.CardNo != null && "" != req.CardNo)
            {
                var card = await _dcr.Find(a => a.CardNo == req.CardNo).CountAsync();
                if (card > 0)
                {
                    return new BaseResponse { Success = false, Message = "输入的卡号被占用" };
                }
            }

            try
            {
                var entity = _mapper.Map<DeviceCardModel>(req);
                entity.DeviceSn = DeviceSn;
                entity.Create = account;
                await _dcr.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}的流量卡成功");
                return new HandleResponse<int> { Success = true, Message = "添加流量卡成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加流量卡数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加流量卡失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateDeviceCardAsync(string account, DeviceCardUpdateDto req, string DeviceSn)
        {
            var d = await _dcr.Find(a => a.DeviceSn == DeviceSn).FirstOrDefaultAsync();
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "该设备没有添加相关的数据，请添加" };
            }
            //检查该设备是否已存在相同的流量卡
            var card = await _dcr.Find(a => a.CardNo == req.CardNo && a.DeviceSn != DeviceSn).CountAsync();
            if (card > 0)
            {
                return new BaseResponse { Success = false, Message = "输入的卡号已被占用，请确认" };
            }
            try
            {
                var entity = _mapper.Map(req, d);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _dcr.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.CardNo}的流量卡信息成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改流量卡失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改流量卡失败，请联系管理员" };
            }
        }

        /// <summary>
        /// 更新流量卡的定位、IMEI和ICCID数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="DeviceSn">设备序列号</param>
        /// <param name="req">流量卡的定位等信息</param>
        /// <returns>返回是否更新成功</returns>
        public async Task<BaseResponse> UpdateDeviceCardPositionAsync(string account, string DeviceSn, DeviceCardPositionUpdateDto req)
        {
            try
            {
                var d = await _dcr.Find(a => a.DeviceSn == DeviceSn).FirstOrDefaultAsync();
                if (d == null)
                {
                    //不存在，就添加
                    var entity = _mapper.Map<DeviceCardModel>(req);
                    entity.DeviceSn = DeviceSn;
                    entity.Create = account;
                    await _dcr.AddAsync(entity);
                    _log.LogInformation($"修改流量卡数据时不存在，{account}添加标示为{entity.Id}的流量卡成功");
                    return new BaseResponse { Success = true, Message = "修改数据成功" };
                }
                else
                {
                    var entity = _mapper.Map(req, d);
                    entity.Modify = account;
                    entity.ModifyTime = DateTime.Now;
                    await _dcr.SaveAsync(entity);
                    _log.LogInformation($"{account}修改标示为{entity.Id}的流量卡信息成功");
                    return new BaseResponse { Success = true, Message = "修改数据成功" };
                }

            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改流量卡失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改流量卡失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteDeviceCardAsync(string account, int Id)
        {
            //检查该设备是否已存在相同的流量卡
            var d = await _dcr.FindAsync(Id);
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "输入的卡号不存在" };
            }
            try
            {
                await _dcr.RemoveAsync(d);
                _log.LogInformation($"{account}删除编号为{Id}流量卡成功");
                return new BaseResponse { Success = true, Message = "删除流量卡数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除编号为{Id}流量卡失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除流量卡失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> GetDeviceCardsAsync(string deviceSn)
        {
            //检查该设备是否已存在相同的流量卡
            var d = await _dcr.Find(a => a.DeviceSn == deviceSn).FirstOrDefaultAsync();
            var dto = _mapper.Map<DeviceCardDto>(d);
            return new BResponse<DeviceCardDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
    }
}
