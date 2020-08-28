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
    [Route("api/{DeviceSn}/[controller]")]
    [ApiController]
    [Authorize]
    public class WarnController : ControllerBase
    {
        private readonly IWarnService _ws;

        public WarnController(IWarnService ws)
        {
            this._ws = ws;
        }

        //获取单个报警信息
        [HttpGet("{Id}")]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetWarnById(string DeviceSn, int Id)
        {
            throw new NotImplementedException();
        }
        //获取设备的报警信息(设备报警)
        public async Task<ActionResult<BaseResponse>> GetDeviceWarn(string DeviceSn)
        {
            throw new NotImplementedException();

        }
        //获取项目或者场站的报警信息
        public async Task<ActionResult<BaseResponse>> GetProjectWarn()
        {
            throw new NotImplementedException();

        }
        //获取报警统计信息（项目、场站或者全部),按类型统计个数
        public async Task<ActionResult<BaseResponse>> GetMyWarnStatistics()
        {
            throw new NotImplementedException();

        }
    }
}