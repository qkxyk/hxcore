using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OpsItemController : ControllerBase
    {
        private readonly ILogger<OpsItemController> _logger;
        private readonly IOpsItemService _ops;

        public OpsItemController(ILogger<OpsItemController> logger, IOpsItemService ops)
        {
            this._logger = logger;
            this._ops = ops;
        }
        /// <summary>
        /// 添加巡检项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddOpsItemAsync([FromBody] OpsItemAddDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _ops.AddOpsItemAsync(account, req);
            return ret;
        }
        /// <summary>
        /// 更新巡检项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateOpsItemAsync([FromBody] OpsItemUpdateDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _ops.UpdateOpsItemAsync(account, req);
            return ret;
        }
        /// <summary>
        /// 删除巡检项目
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteOpsItemAsync(int Id)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _ops.DeleteOpsItemAsync(account, Id);
            return ret;
        }
        /// <summary>
        /// 获取全部巡检项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetAllOpsItemAsync([FromQuery] BaseRequest req)
        {
            var ret = await _ops.GetOpsItemAsync(req);
            return ret;
        }
        /// <summary>
        /// 获取分页巡检项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("Page")]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetPageOpsItemAsync([FromQuery] BasePageRequest req)
        {
            var ret = await _ops.GetOpsItemPageAsync(req);
            return ret;
        }
    }
}
