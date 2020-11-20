using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
using HXCloud.ViewModel;
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
        private readonly IDeviceHisDataService _dhs;
        private readonly IDeviceService _ds;

        public DeviceHisDataController(IDeviceHisDataService dhs, IDeviceService ds)
        {
            this._dhs = dhs;
            this._ds = ds;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetDeviceHisDataAsync(string GroupId, string DeviceSn, [FromQuery] DeviceHisDataPageRequest req)
        {
            throw new NotImplementedException();
        }
    }
}