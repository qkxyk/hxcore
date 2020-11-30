using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/{GroupId}/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceStatisticsDataController : ControllerBase
    {
        private readonly IDeviceStatistcsDataService _dsd;
        private readonly IGroupService _gs;

        public DeviceStatisticsDataController(IDeviceStatistcsDataService dsd,IGroupService gs)
        {
            this._dsd = dsd;
            this._gs = gs;
        }

        public async Task<ActionResult<BaseResponse>> GetDeviceStatisticsAsync(string GroupId,[FromQuery]DeviceStatisticsRequestDto req)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //验证输入的groupid是否存在
            var ex = await _gs.IsExist(a => a.Id == GroupId);
            if (!ex)
            {
                return new BaseResponse { Success = false, Message = "输入的组织编号不存在" };
            }
        }
    }
}