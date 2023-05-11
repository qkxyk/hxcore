using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OpsStatisticsController : ControllerBase
    {
        private readonly IUserService _user;
        private readonly IOpsStatisticsService _ops;

        public OpsStatisticsController(IUserService user, IOpsStatisticsService ops)
        {
            this._user = user;
            this._ops = ops;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetOpsDataAsync([FromQuery] OpsStatisticsRequest req)
        {
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            //运维人员
            var ops = User.Claims.FirstOrDefault(a => a.Type == "Category").Value;
            int category = 0;
            int.TryParse(ops, out category);
            var users = await _user.GetUserAndChildAsync(Account, isAdmin);
            var ret = await _ops.GetOpsStatisticsAsync(users.Keys.ToList(), req);
            return ret;
        }
    }
}
