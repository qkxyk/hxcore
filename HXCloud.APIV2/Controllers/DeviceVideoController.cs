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
    public class DeviceVideoController : ControllerBase
    {
        private readonly DeviceVideoService _dvs;

        public DeviceVideoController(DeviceVideoService dvs)
        {
            this._dvs = dvs;
        }
        [HttpPost]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddDeviceVideo(string deviceSn, DeviceVideoAddDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dvs.AddDeviceVideoAsync(account, req, deviceSn);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateDeviceVideo(string deviceSn, DeviceVideoUpdateDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dvs.UpdateDeviceVideoAsync(account, req, deviceSn);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteDeviceVideo(string deviceSn, int Id)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dvs.DeleteDeviceVideoAsync(account, Id);
            return rm;
        }
        [HttpGet("{Id}")]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceVideo(string deviceSn, int Id)
        {
            var rm = await _dvs.GetDeviceVideoAsync(Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceVideos(string deviceSn)
        {
            var rm = await _dvs.GetDeviceVideoesAsync(deviceSn);
            return rm;
        }
    }
}