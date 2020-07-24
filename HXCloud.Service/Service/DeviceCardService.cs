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
            //检查该设备是否已存在相同的流量卡
            var d = await _dcr.Find(a => a.CardNo == req.CardNo).ToListAsync();
            if (d.Count > 0)
            {
                return new BaseResponse { Success = false, Message = "输入的卡号被占用" };
            }
            try
            {
                var entity = _mapper.Map<DeviceCardModel>(req);
                entity.DeviceSn = DeviceSn;
                entity.Create = account;
                await _dcr.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{req.CardNo}的流量卡成功");
                return new HandleResponse<string> { Success = true, Message = "添加流量卡成功", Key = req.CardNo };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加流量卡失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加流量卡失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateDeviceCardAsync(string account, DeviceCardUpdateDto req, string DeviceSn)
        {
            //检查该设备是否已存在相同的流量卡
            var d = await _dcr.FindAsync(req.CardNo);
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "输入的卡号被不存在" };
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
        public async Task<BaseResponse> DeleteDeviceCardAsync(string account, string cardNo)
        {
            //检查该设备是否已存在相同的流量卡
            var d = await _dcr.FindAsync(cardNo);
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "输入的卡号被不存在" };
            }
            try
            {
                await _dcr.RemoveAsync(d);
                _log.LogInformation($"{account}删除卡号为{cardNo}流量卡成功");
                return new BaseResponse { Success = true, Message = "删除流量卡数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除流量卡失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除流量卡失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> GetDeviceCardAsync(string cardNo)
        {
            //检查该设备是否已存在相同的流量卡
            var d = await _dcr.FindAsync(cardNo);
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "输入的卡号被不存在" };
            }
            var dto = _mapper.Map<DeviceCardDto>(d);
            return new BResponse<DeviceCardDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetDeviceCardsAsync(string deviceSn)
        {
            //检查该设备是否已存在相同的流量卡
            var d = await _dcr.Find(a => a.DeviceSn == deviceSn).ToListAsync();
            //if (d.Count<=0)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的卡号被不存在" };
            //}
            var dtos = _mapper.Map<List<DeviceCardDto>>(d);
            return new BResponse<List<DeviceCardDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
    }
}
