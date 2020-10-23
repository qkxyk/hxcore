using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/{GroupId}/{DeviceSn}/[controller]")]
    [ApiController]
    public class DeviceHardwareController : ControllerBase
    {
        private readonly IDeviceHardwareConfigService _dhc;

        public DeviceHardwareController(IDeviceHardwareConfigService dhc)
        {
            this._dhc = dhc;
        }
        [HttpPost]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddDeviceHardwareAsync(string GroupId, string DeviceSn, [FromBody] DeviceHardwareConfigAddDto req)
        {
            throw new NotImplementedException();
        }
        [HttpPut]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateDeviceHardwareAsync(string GroupId, string DeviceSn, [FromBody] DeviceHardwareConfigUpdateDto req)
        {
            throw new NotImplementedException();
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteDeviceHarewareAsync(string GroupId, string DeviceSn, int Id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{Id}")]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceHardwareByIdAsync(string GroupId, string DeviceSn, int Id)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceHarewareAsync(string GroupId, string DeviceSn)
        {
            throw new NotImplementedException();
        }
    }
}