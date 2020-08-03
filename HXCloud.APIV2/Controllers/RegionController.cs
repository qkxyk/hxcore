using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/{GroupId}/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _rs;
        private readonly IConfiguration _config;

        public RegionController(IRegionService rs, IConfiguration config)
        {
            this._rs = rs;
            this._config = config;
        }

        [HttpPost]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> RegionAdd(string GroupId, RegionAddDto req)
        {
            //管理员权限
            //var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            //管理员和超级管理员有权限
            //if (true)
            //{

            //}
            //if (!isAdmin)
            //{
            //    return Unauthorized("用户没有权限添加区域");
            //}
            //else if (GroupId != GId && Code != _config["Group"])
            //{
            //    return Unauthorized("用户没有权限添加区域");
            //}
            var rm = await _rs.AddRegionAsync(Account, GroupId, req);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> RegionUpdate(string GroupId, RegionUpdateDto req)
        {
            //管理员权限
            //var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //if (!isAdmin)
            //{
            //    return Unauthorized("用户没有权限编辑区域");
            //}
            var rm = await _rs.UpdateRegionAsync(Account, GroupId, req);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> RegionDelete(string GroupId, string Id)
        {
            //管理员权限
            //var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //if (!isAdmin)
            //{
            //    return Unauthorized("用户没有权限删除区域");
            //}
            var rm = await _rs.DeleteRegionAsync(Account, Id, GroupId);
            return rm;
        }
        [HttpGet("{Id}")]
        [TypeFilter(typeof(GroupOrAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> RegionGet(string GroupId, string Id)
        {
            //var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _rs.GetRegionAsync(Id, GroupId);
            return rm;
        }
        //返回数据数组形式
        [HttpGet("Child/{Id}")]
        [TypeFilter(typeof(GroupOrAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> RegionGetWithChild(string GroupId, string Id)
        {
            var rm = await _rs.GetRegionWithChildAsync(Id, GroupId);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(GroupOrAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> RegionGetAll(string GroupId)
        {
            //var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _rs.GetGroupRegionAsync(GroupId);
            return rm;
        }
    }
}