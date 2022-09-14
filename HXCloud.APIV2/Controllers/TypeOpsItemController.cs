using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/type/{typeId}/[controller]")]
    [ApiController]
    public class TypeOpsItemController : ControllerBase
    {
        private readonly ITypeOpsItemService _typeOpsItem;

        public TypeOpsItemController(ITypeOpsItemService typeOpsItem)
        {
            this._typeOpsItem = typeOpsItem;
        }
        /// <summary>
        /// 添加类型巡检项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddOpsItemAsync(int typeId,[FromBody] TypeOpsItemAddDto req)
        {
            if (typeId!=req.TypeId)
            {
                return new BaseResponse { Success = false, Message = "类型标识不一致" };
            }
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _typeOpsItem.AddOpsItemAsync(account, req);
            return ret;
        }
        /// <summary>
        /// 更新类型巡检项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateOpsItemAsync([FromBody] TypeOpsItemUpdateDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _typeOpsItem.UpdateOpsItemAsync(account, req);
            return ret;
        }
        /// <summary>
        /// 删除类型巡检项目
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteOpsItemAsync(int Id)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _typeOpsItem.DeleteOpsItemAsync(account, Id);
            return ret;
        }
        /// <summary>
        /// 获取全部类型巡检项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetAllOpsItemAsync([FromQuery] BaseRequest req, int typeId)
        {
            var ret = await _typeOpsItem.GetOpsItemAsync(req, typeId);
            return ret;
        }
        /// <summary>
        /// 获取分页类型巡检项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("Page")]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetPageOpsItemAsync(int typeId, [FromQuery] BasePageRequest req)
        {
            var ret = await _typeOpsItem.GetOpsItemPageAsync(req, typeId);
            return ret;
        }
    }
}
