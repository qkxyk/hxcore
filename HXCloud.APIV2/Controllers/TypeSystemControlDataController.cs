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
    [Route("api/systemAccessory/{accessoryId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeSystemControlDataController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITypeSystemAccessoryService _tsas;
        private readonly ITypeSystemAccessoryControlDataService _tscs;
        private readonly ITypeDataDefineService _tds;

        public TypeSystemControlDataController(IConfiguration config, ITypeSystemAccessoryService tsas, ITypeSystemAccessoryControlDataService tscs, ITypeDataDefineService tds)
        {
            this._config = config;
            this._tsas = tsas;
            this._tscs = tscs;
            this._tds = tds;
        }
        [HttpPost]
        [TypeFilter(typeof(TypeSystemAccessoryFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddSystemControlDataAsync(int accessoryId, TypeSystemAccessoryControlDataAddDto req)
        {
            bool IsExist = await _tds.IsExist(a => a.Id == req.DataDefineId);
            if (!IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的类型数据定义不存在" };
            }
            if (req.AssociateDefineId != 0)
            {
                IsExist = await _tds.IsExist(a => a.Id == req.AssociateDefineId);
                if (!IsExist)
                {
                    return new BaseResponse { Success = false, Message = "输入的关联类型数据定义不存在" };
                }
            }
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tsas.IsExist(a => a.Id == accessoryId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型子系统不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tscs.AddSystemControlDataAsync(accessoryId, req, Account);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(TypeSystemAccessoryFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> TypeSystemAccessoryControlDataUpdate(int accessoryId, TypeSystemAccessoryControlDataUpdateDto req)
        {
            bool IsExist = await _tds.IsExist(a => a.Id == req.DataDefineId);
            if (!IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的类型数据定义不存在" };
            }
            if (req.AssociateDefineId != 0)
            {
                IsExist = await _tds.IsExist(a => a.Id == req.AssociateDefineId);
                if (!IsExist)
                {
                    return new BaseResponse { Success = false, Message = "输入的关联类型数据定义不存在" };
                }
            }
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tsas.IsExist(a => a.Id == accessoryId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型子系统不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tscs.UpdateSystemAccessoryControlDataAsync(accessoryId, req, Account);
            return rm;
        }

        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeSystemAccessoryFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> SystemAccessoryCotrolDataDelete(int accessoryId, int Id)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tsas.IsExist(a => a.Id == accessoryId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型子系统不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tscs.DeleteSystemAccessoryControlDataAsync(Id, Account);
            return rm;
        }
        [HttpGet("{Id}")]
        [TypeFilter(typeof(TypeSystemAccessoryFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetSystemControlData(int accessoryId, int Id)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tsas.IsExist(a => a.Id == accessoryId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型子系统不存在" };
            //}
            ////用户所在的组和超级管理员可以查看
            //if (um.GroupId != GroupId || (!um.IsAdmin && um.Code != _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var rm = await _tscs.GetControlDataAsync(Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(TypeSystemAccessoryFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetAccessoryControlData(int accessoryId)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tsas.IsExist(a => a.Id == accessoryId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型子系统不存在" };
            //}
            ////用户所在的组和超级管理员可以查看
            //if (um.GroupId != GroupId || (!um.IsAdmin && um.Code != _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var rm = await _tscs.GetAccessoryControlDataAsync(accessoryId);
            return rm;
        }
    }
}