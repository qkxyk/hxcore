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
    /// <summary>
    /// 增加一个获取一天历史数据的接口，根据日期
    /// </summary>
    [Route("api/{GroupId}/{DeviceSn}/[controller]")]
    [ApiController]
    [Authorize]
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
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceHisDataAsync(string GroupId, string DeviceSn, [FromQuery] DeviceHisDataPageRequest req)
        {
            var ret = await _dhs.GetDeviceHisDataAsync(DeviceSn, req);
            return ret;
        }
        /// <summary>
        /// 获取设备最新一条历史数据
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="DeviceSn"></param>
        /// <returns></returns>
        [HttpGet("Latest")]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceLatestHisDataAsync(string GroupId, string DeviceSn,int order=0)
        {
            var ret = await _dhs.GetDeviceLatestHisDataAsync(DeviceSn,order);
            return ret;
        }
    }
}