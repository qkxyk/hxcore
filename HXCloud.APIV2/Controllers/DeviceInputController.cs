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
    public class DeviceInputController : ControllerBase
    {
        private readonly IDeviceInputDataService _dis;

        public DeviceInputController(IDeviceInputDataService dis)
        {
            this._dis = dis;
        }
        [HttpPost]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddDeviceInputData(string deviceSn, DeviceInputAddDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dis.AddDeviceInputDataAsync(account, req, deviceSn);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateDeviceInputData(string deviceSn, DeviceInputDataUpdateDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dis.UpdateDeviceInputDataAsync(account, req, deviceSn);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteDeviceInputData(string deviceSn, int Id)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dis.DeleteDeviceInputDataAsync(account, Id);
            return rm;
        }
        [HttpGet("{Id}")]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceInputData(string deviceSn, int Id)
        {
            var rm = await _dis.GetDeviceInputDataAsync(Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceInputDatas(string deviceSn)
        {
            var rm = await _dis.GetAllDeviceInputDataAsync(deviceSn);
            return rm;
        }
    }
}