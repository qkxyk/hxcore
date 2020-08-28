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
    [Route("api/{GroupId}/{DeviceSn}/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceVideoController : ControllerBase
    {
        private readonly IDeviceVideoService _dvs;

        public DeviceVideoController(IDeviceVideoService dvs)
        {
            this._dvs = dvs;
        }
        [HttpPost]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddDeviceVideo(string GroupId, string DeviceSn, DeviceVideoAddDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dvs.AddDeviceVideoAsync(account, req, DeviceSn);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateDeviceVideo(string GroupId, string DeviceSn, DeviceVideoUpdateDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dvs.UpdateDeviceVideoAsync(account, req, DeviceSn);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteDeviceVideo(string GroupId, string DeviceSn, int Id)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dvs.DeleteDeviceVideoAsync(account, Id);
            return rm;
        }
        [HttpGet("{Id}")]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceVideo(string GroupId, string DeviceSn, int Id)
        {
            var rm = await _dvs.GetDeviceVideoAsync(Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceVideos(string GroupId, string DeviceSn)
        {
            var rm = await _dvs.GetDeviceVideoesAsync(DeviceSn);
            return rm;
        }
        [HttpPut("Refresh/{Id}")]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> RefreshVideoToken(string GroupId, string DeviceSn, int Id)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dvs.GetVideoTokenAsync(account, Id);
            return rm;
        }
    }
}