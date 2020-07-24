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

namespace HXCloud.APIV2.Controllers
{
    [Route("api/device/{deviceSn}/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceConfigController : ControllerBase
    {
        private readonly IDeviceConfigService _dcs;

        public DeviceConfigController(IDeviceConfigService dcs)
        {
            this._dcs = dcs;
        }
        [HttpPost]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddDeviceConfig(string deviceSn, DeviceConfigAddDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dcs.AddDeviceConfigAsync(account, req, deviceSn);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateDeviceConfig(string deviceSn, DeviceConfigUpdateDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dcs.UpdateDeviceConfigAsync(account, req, deviceSn);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteDeviceConfig(string deviceSn, int Id)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dcs.DeleteDeviceConfigAsync(account, Id);
            return rm;
        }
        [HttpGet("{Id}")]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceConfig(string deviceSn, int Id)
        {
            var rm = await _dcs.GetDeviceConfigAsync(Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceConfigs(string deviceSn)
        {
            var rm = await _dcs.GetDeviceConfigsAsync(deviceSn);
            return rm;
        }
    }
}