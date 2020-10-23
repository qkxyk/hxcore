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
    [Route("api/{GroupId}/{DeviceSn}[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceLogController : ControllerBase
    {
        private readonly IDeviceLogService _dls;

        public DeviceLogController(IDeviceLogService dls)
        {
            this._dls = dls;
        }

        [HttpPost]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddDeviceLogAsync(string GroupId, string DeviceSn, [FromBody] DeviceLogAddDto req)
        {
            throw new NotImplementedException();
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteDeviceLogAsync(string GroupId, string DeviceSn, int Id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceLogAsync(string GroupId, string DeviceSn, [FromQuery] DeviceLogPageRequest req)
        {
            throw new NotImplementedException();
        }
    }
}