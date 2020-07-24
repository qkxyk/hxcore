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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/accessory/{accessoryId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeAccessoryControlDataController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITypeAccessoryControlDataService _tacs;
        private readonly ITypeDataDefineService _tds;
        private readonly ITypeAccessoryService _tas;

        public TypeAccessoryControlDataController(IConfiguration config, ITypeAccessoryControlDataService tacs, ITypeDataDefineService tds, ITypeAccessoryService tas)
        {
            this._config = config;
            this._tacs = tacs;
            this._tds = tds;
            this._tas = tas;
        }
        [HttpPost]
        [TypeFilter(typeof(TypeAccessoryFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddAsync(int accessoryId, TypeControlDataAddDto req)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tas.IsExist(a => a.Id == accessoryId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型配件不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            //验证数据定义标示是否存在
            var data = await _tds.IsExist(a => a.Id == req.DataDefineId);
            if (!data)
            {
                return new BaseResponse { Success = false, Message = "输入的数据定义标示不存在" };
            }
            //验证关联的数据定义是否存在
            if (req.AssociateDefineId != 0)
            {
                data = await _tds.IsExist(a => a.Id == req.AssociateDefineId);
                if (!data)
                {
                    return new BaseResponse { Success = false, Message = "输入的关联数据定义标示不存在" };
                }
            }
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tacs.AddAccessoryControlData(accessoryId, req, Account);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(TypeAccessoryFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> TypeAccessoryControlUpdate(int accessoryId, TypeControlDataUpdateDto req)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tas.IsExist(a => a.Id == accessoryId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型配件不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tacs.UpdateTypeControlDataAsync(accessoryId, req, Account);
            return rm;
        }

        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeAccessoryFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> TypeAccessoryControlDataDelete(int accessoryId, int Id)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tas.IsExist(a => a.Id == accessoryId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型配件不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tacs.DeleteTypeAccessoryControlDataAsync(Id, Account);
            return rm;
        }
        [HttpGet("{Id}")]
        [TypeFilter(typeof(TypeAccessoryFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetControlData(int accessoryId, int Id)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tas.IsExist(a => a.Id == accessoryId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型配件不存在" };
            //}
            ////用户所在的组和超级管理员可以查看
            //if (um.GroupId != GroupId || (!um.IsAdmin && um.Code != _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var rm = await _tacs.GetControlDataAsync(Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(TypeAccessoryFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetAccessoryControl(int accessoryId)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tas.IsExist(a => a.Id == accessoryId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型配件不存在" };
            //}
            ////用户所在的组和超级管理员可以查看
            //if (um.GroupId != GroupId || (!um.IsAdmin && um.Code != _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var rm = await _tacs.GetAccessoryControlDataAsync(accessoryId);
            return rm;
        }
    }
}