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
    public class DeviceStatisticsDataController : ControllerBase
    {
        private readonly IDeviceStatistcsDataService _dsd;

        public DeviceStatisticsDataController(IDeviceStatistcsDataService dsd)
        {
            this._dsd = dsd;
        }
    }
}