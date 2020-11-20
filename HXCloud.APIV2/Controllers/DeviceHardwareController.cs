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
    public class DeviceHardwareController : ControllerBase
    {
        private readonly IDeviceHardwareConfigService _dhc;
        private readonly IDeviceService _ds;

        public DeviceHardwareController(IDeviceHardwareConfigService dhc, IDeviceService ds)
        {
            this._dhc = dhc;
            this._ds = ds;
        }
        [HttpPost]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddDeviceHardwareAsync(string GroupId, string DeviceSn, [FromBody] DeviceHardwareConfigAddDto req)
        {
            var device = await _ds.IsExistCheck(a => a.DeviceSn == DeviceSn);
            if (!device.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在" };
            }
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dhc.AddDeviceHardwareConfigAsync(Account, DeviceSn, req);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateDeviceHardwareAsync(string GroupId, string DeviceSn, [FromBody] DeviceHardwareConfigUpdateDto req)
        {
            var device = await _ds.IsExistCheck(a => a.DeviceSn == DeviceSn);
            if (!device.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在" };
            }
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dhc.UpdateDeviceHardwareConfigAsync(Account, DeviceSn, req);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteDeviceHarewareAsync(string GroupId, string DeviceSn, int Id)
        {
            var device = await _ds.IsExistCheck(a => a.DeviceSn == DeviceSn);
            if (!device.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在" };
            }
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dhc.DeleteDeviceHardwareConfigAsync(Id, Account);
            return rm;
        }

        [HttpGet("{Id}")]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceHardwareByIdAsync(string GroupId, string DeviceSn, int Id)
        {
            var device = await _ds.IsExistCheck(a => a.DeviceSn == DeviceSn);
            if (!device.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在" };
            }
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dhc.GetHardwareConfigAsync(Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceHarewareAsync(string GroupId, string DeviceSn, [FromQuery] BasePageRequest req)
        {
            var device = await _ds.IsExistCheck(a => a.DeviceSn == DeviceSn);
            if (!device.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在" };
            }
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dhc.GetTypeHardwareConfigAsync(DeviceSn, req);
            return rm;
        }
    }
}