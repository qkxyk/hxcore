using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// 增加一个获取一天历史数据的接口，根据日期
    /// </summary>
    [Route("api/{GroupId}/{DeviceSn}/[controller]")]
    [ApiController]
    public class DeviceHisDataController : ControllerBase
    {
        public DeviceHisDataController()
        {

        }
    }
}