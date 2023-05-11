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
    public class RepairPartController : ControllerBase
    {
        private readonly IRepairPartService _part;
        private readonly IRepairService _repair;
        private readonly IUserService _user;

        public RepairPartController(IRepairPartService part, IRepairService repair, IUserService user)
        {
            this._part = part;
            this._repair = repair;
            this._user = user;
        }
        [HttpPost]
        public async Task<BaseResponse> AddRepairPartAsync(RepairPartAddDto req)
        {
            //只能是接单人添加维修配件
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _repair.IsExistAsync(a => a.Id == req.RepairId);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的工单编号不存在" };
            }
            if (Account != ret.Receiver)
            {
                return new BaseResponse { Success = false, Message = "用户没有权限添加运维配件" };
            }
            //获取用户中文名
            var u = await _user.GetUserByAccountAsync(Account);
            var mess = await _part.AddRepairPartAsync(Account, u.UserName, req);
            return mess;
        }
    }
}
