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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _rs;

        public RegionController(IRegionService rs)
        {
            this._rs = rs;
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> RegionAdd(RegionAddDto req)
        {
            //管理员权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!isAdmin)
            {
                return Unauthorized("用户没有权限添加区域");
            }
            return Ok();

        }
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> RegionUpdate(RegionUpdateDto req)
        {
            //管理员权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!isAdmin)
            {
                return Unauthorized("用户没有权限编辑区域");
            }
            return Ok();
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> RegionDelete(int Id)
        {
            //管理员权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!isAdmin)
            {
                return Unauthorized("用户没有权限删除区域");
            }
            return Ok();
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> RegionGet(int Id)
        {
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            return Ok();

        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> RegionGetAll()
        {
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            return Ok();
        }
    }
}