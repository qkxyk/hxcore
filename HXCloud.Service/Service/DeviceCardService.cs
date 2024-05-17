using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HXCloud.Service
{
    public class DeviceCardService : IDeviceCardService
    {
        private readonly ILogger<DeviceCardService> _log;
        private readonly IMapper _mapper;
        private readonly IDeviceCardRepository _dcr;
        private readonly ISimbossRepository _simboss;
        private readonly IHttpClientFactory _clientFactory;

        public DeviceCardService(ILogger<DeviceCardService> log, IMapper mapper, IDeviceCardRepository dcr, ISimbossRepository simboss, IHttpClientFactory clientFactory)
        {
            this._log = log;
            this._mapper = mapper;
            this._dcr = dcr;
            this._simboss = simboss;
            this._clientFactory = clientFactory;
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
        /// <summary>
        /// 获取设备sim卡信息
        /// </summary>
        /// <param name="deviceSn">设备序列号</param>
        /// <returns>返回设备sim卡数据</returns>
        public async Task<BaseResponse> GetDeviceSimbossAsync(string deviceSn)
        {
            //获取simboss配置信息，多个simboss账号
            var simbosses = await _simboss.Find(a => true).ToListAsync();
            if (simbosses == null)
            {
                return new BaseResponse { Success = false, Message = "系统没有录入simboss数据，请通知管理员录入该数据" };
            }
            //获取设备的simboss卡数据
            var deviceSimboss = await _dcr.Find(a => a.DeviceSn == deviceSn).FirstOrDefaultAsync();
            if (deviceSimboss == null)
            {
                return new BaseResponse { Success = false, Message = "系统没有该设备simboss数据，请先获取设备的simboss数据" };
            }
            //List<Task> tasks = new List<Task>();
            SingelCardResponse ret = null;
            foreach (var simboss in simbosses)
            {

                //组装simboss数据
                string apiUrl = "https://api.simboss.com/2.0/device/detail";//simboss获取单卡的地址
                if (string.IsNullOrEmpty(simboss.AppId) || string.IsNullOrEmpty(simboss.AppSecret))
                {
                    continue;
                    //return new BaseResponse { Success = false, Message = "录入simboss数据有误，请通知管理员录入该数据" };
                }
                string appid = simboss.AppId;// "102420143446";
                string AppSeret = simboss.AppSecret;// "283ebfc1ed3461512393ca35fe214e60";
                string timestamp = SimBossInfo.GetTimeStamp(DateTime.Now).ToString();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("appid", appid);
                dic.Add("timestamp", timestamp);
                if (string.IsNullOrEmpty(deviceSimboss.ICCID))
                {
                    return new BaseResponse { Success = false, Message = "请该设备的sim卡卡号不存在，请先获取sim卡卡号" };
                }
                dic.Add("iccid", deviceSimboss.ICCID);
                string hex = SimBossInfo.CreateSign(dic, AppSeret);
                string sign = SimBossInfo.sha256(hex);

                Dictionary<string, string> dicParams = new Dictionary<string, string>();
                dicParams.Add("appid", appid);
                dicParams.Add("timestamp", timestamp);
                dicParams.Add("sign", sign);
                dicParams.Add("iccid", deviceSimboss.ICCID);
                var client = _clientFactory.CreateClient();
                string data = SimBossInfo.CreateParams(dicParams);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");

                using (var httpResponse = await client.PostAsync(apiUrl, content))
                {
                    var message = httpResponse.EnsureSuccessStatusCode();
                    var ms = await message.Content.ReadAsByteArrayAsync();
                    string mes = Encoding.UTF8.GetString(ms);
                    var cardData = JsonConvert.DeserializeObject<SingelCardResponse>(mes);
                    if (cardData!=null&&cardData.code!="404"||cardData.data!=null)
                    {
                        ret = cardData;
                        break;
                    }
                    //return new BResponse<CardResponse> { Success = true, Message = "获取数据成功", Data = cardData.data };
                }
            }
            return new BResponse<CardResponse> { Success = true, Message = "获取数据成功", Data =ret==null ? null: ret.data };
        }
    }

    public static class SimBossInfo
    {
        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="DicData">参加运算的数据</param>
        /// <param name="secret">用户的密钥</param>
        /// <returns>返回签名数据</returns>
        public static string CreateSign(Dictionary<string, string> DicData, string secret)
        {
            var dicSort = from objDic in DicData orderby objDic.Key ascending select objDic;
            StringBuilder sb = new StringBuilder();
            foreach (var item in dicSort)
            {
                sb.Append($"{item.Key}={item.Value}");
                if (item.Key != dicSort.Last().Key)
                {
                    sb.Append("&");
                }
            }
            sb.Append(secret);
            return sb.ToString();
        }
        /// <summary>
        /// 组合参数
        /// </summary>
        /// <param name="DicData">要组合的参数列表</param>
        /// <returns>返回组合后的参数</returns>
        public static string CreateParams(Dictionary<string, string> DicData)
        {
            var dicSort = from objDic in DicData orderby objDic.Key ascending select objDic;
            StringBuilder sb = new StringBuilder();
            foreach (var item in dicSort)
            {
                sb.Append($"{item.Key}={item.Value}");
                if (item.Key != dicSort.Last().Key)
                {
                    sb.Append("&");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// sha256计算
        /// </summary>
        /// <param name="data">要计算的数据</param>
        /// <returns></returns>
        public static string sha256(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }

            return builder.ToString();
        }

        public static DateTime GetDateTime()
        {
            return default;
        }

        /// <summary>
        /// 日期转换为时间戳Timestamp
        /// </summary>
        /// <param name="dateTime">要转换的日期</param>
        /// <param name="len">时间戳长度</param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime dateTime, TimeLen len = TimeLen.Thirteen)
        {
            DateTime _dtStart = new DateTime(1970, 1, 1, 8, 0, 0);
            long timeStamp = 0;
            if (len == TimeLen.Ten)
            {
                //10位的时间戳
                timeStamp = Convert.ToInt32(dateTime.Subtract(_dtStart).TotalSeconds);
            }
            else
            {
                //13位的时间戳
                timeStamp = Convert.ToInt64(dateTime.Subtract(_dtStart).TotalMilliseconds);
            }
            return timeStamp;
        }
        /// <summary>
        /// UTC时间戳Timestamp转换为北京时间
        /// </summary>
        /// <param name="timestamp">要转换的时间戳</param>
        /// <param name="len">时间戳长度</param>
        /// <returns></returns>
        public static DateTime GetDateTime(long timestamp, TimeLen len = TimeLen.Thirteen)
        {
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); 
            //使用上面的方式会显示TimeZone已过时
            try
            {
                DateTime dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
                long lTime = 0;
                if (len == TimeLen.Ten)
                {
                    lTime = long.Parse(timestamp + "0000000");
                }
                else
                {
                    lTime = long.Parse(timestamp + "0000");
                }

                TimeSpan timeSpan = new TimeSpan(lTime);
                //TimeSpan timeSpan = new TimeSpan(timestamp);
                DateTime targetDt = dtStart.Add(timeSpan).AddHours(8);
                return targetDt;
            }
            catch (Exception ex)
            {
                return default;//{0001/1/1 0:00:00}
            }

        }

        //时间戳长度
        public enum TimeLen
        {//10位和13位
            Ten, Thirteen
        }
    }
}

