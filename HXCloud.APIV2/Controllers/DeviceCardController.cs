﻿using System;
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
    [Route("api/device/{deviceSn}/[controller]")]
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
        public async Task<ActionResult<BaseResponse>> AddDeviceCard(string devicesn, [FromBody]DeviceCardAddDto req)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //添加流量卡
            var rm = await _dcs.AddDeviceCardAsync(devicesn, req, account);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateDeviceCard(string devicesn, [FromBody] DeviceCardUpdateDto req)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dcs.UpdateDeviceCardAsync(account, req, devicesn);
            return rm;
        }
        [HttpDelete("{CardNo}")]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteDeviceCard(string devicesn, string CardNo)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dcs.DeleteDeviceCardAsync(account, CardNo);
            return rm;
        }

        [HttpGet("{CardNo}")]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceCard(string deviceSn, string CardNo)
        {
            var rm = await _dcs.GetDeviceCardAsync(CardNo);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceCards(string deviceSn)
        {
            var rm = await _dcs.GetDeviceCardsAsync(deviceSn);
            return rm;
        }
    }
}