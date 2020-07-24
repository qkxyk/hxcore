using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/device/{deviceSn}/[controller]")]
    [ApiController]
    public class DeviceHardwareController : ControllerBase
    {
    }
}