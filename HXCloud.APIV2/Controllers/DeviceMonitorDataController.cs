using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/{DeviceSn}/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceMonitorDataController : ControllerBase
    {
        private readonly IDeviceMonitorDataService _dmds;
        private readonly IDeviceService _ds;

        public DeviceMonitorDataController(IDeviceMonitorDataService dmds,IDeviceService ds)
        {
            this._dmds = dmds;
            this._ds = ds;
        }
        [HttpGet]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDevcieMonitorData(string DeviceSn, [FromQuery]DeviceMonitorDataRequestDto req)
        {
            var device = await _ds.IsExistCheck(a => a.DeviceSn == DeviceSn);
            if (!device.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在" };
            }
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dmds.GetDeviceMonitorAsync(DeviceSn, req);
            return rm;
        }
    }
}
