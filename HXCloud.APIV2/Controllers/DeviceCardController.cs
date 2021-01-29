using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// 一个设备对应一个流量卡
    /// </summary>
    [Authorize]
    [Route("api/{GroupId}/{DeviceSn}/[controller]")]
    [ApiController]
    public class DeviceCardController : ControllerBase
    {
        //private readonly IConfiguration _config;
        //private readonly IDeviceService _ds;
        private readonly IDeviceCardService _dcs;
        //private readonly IRoleProjectService _rs;

        public DeviceCardController(/*IConfiguration config, IDeviceService ds, */IDeviceCardService dcs/*, IRoleProjectService rs*/)
        {
            //this._config = config;
            //this._ds = ds;
            this._dcs = dcs;
            //this._rs = rs;
        }

        [HttpPost]
        //[ServiceFilter(typeof(DeviceActionFilterAttribute))]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddDeviceCard(string GroupId, string DeviceSn, [FromBody] DeviceCardAddDto req)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //添加流量卡
            var rm = await _dcs.AddDeviceCardAsync(DeviceSn, req, account);
            return rm;
        }
        [HttpPut("Card")]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateDeviceCard(string GroupId, string DeviceSn, [FromBody] DeviceCardUpdateDto req)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dcs.UpdateDeviceCardAsync(account, req, DeviceSn);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateDevicePosition(string GroupId, string DeviceSn, [FromBody] DeviceCardPositionUpdateDto req)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dcs.UpdateDeviceCardPositionAsync(account, DeviceSn, req);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteDeviceCard(string GroupId, string DeviceSn, int Id)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dcs.DeleteDeviceCardAsync(account, Id);
            return rm;
        }

        [HttpGet]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceCards(string GroupId, string DeviceSn)
        {
            var rm = await _dcs.GetDeviceCardsAsync(DeviceSn);
            return rm;
        }
    }
}