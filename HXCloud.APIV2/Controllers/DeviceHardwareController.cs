using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
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
    }
}