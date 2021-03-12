using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// 类型模块配置数据
    /// </summary>
    [Route("api/Module/{ModuleId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeModuleArgumentController : ControllerBase
    {
        private readonly ITypeDataDefineService _td;
        private readonly IConfiguration _config;
        private readonly ITypeModuleService _tms;
        private readonly ITypeModuleArgumentService _tma;

        /// <summary>
        /// 初始化类型模块配置数据
        /// </summary>
        /// <param name="td">注入类型数据定义</param>
        /// <param name="tms">注入模块数据</param>
        /// <param name="tma">注入模块配置数据</param>
        public TypeModuleArgumentController(ITypeDataDefineService td, IConfiguration config, ITypeModuleService tms, ITypeModuleArgumentService tma)
        {
            this._td = td;
            this._config = config;
            this._tms = tms;
            this._tma = tma;
        }

        /// <summary>
        /// 添加模块配置数据
        /// </summary>
        /// <param name="ModuleId">模块标识</param>
        /// <param name="req">配置数据</param>
        /// <returns>添加模块配置数据是否成功</returns>
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddTypeModuleArgumentAsync(int ModuleId, [FromBody] TypeModuleArgumentAddDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //只有该组织的管理员和超级管理员有权限
            if (!isAdmin)
            {
                return new BaseResponse { Success = false, Message = "用户没有权限添加模块的配置数据" };
            }
            var data = await _tma.IsExistCheck(a => a.Id == ModuleId);
            if (!data.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的模块编号不存在" };
            }
            if (GroupId != data.GroupId && Code != _config["Group"])
            {
                return Unauthorized("用户没有权限");
            }
            var ret = await _tma.AddTypeModuleArgumentAsync(Account, ModuleId, data.TypeId, req);
            return ret;
        }
        /// <summary>
        /// 修改模块配置数据
        /// </summary>
        /// <param name="ModuleId">模块标识</param>
        /// <param name="req">模块配置数据信息</param>
        /// <returns>返回修改数据是否成功的信息</returns>
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> UpdateTypeModuleArgumentAsync(int ModuleId, [FromBody] TypeModuleArgumentUpdateDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //只有该组织的管理员和超级管理员有权限
            if (!isAdmin)
            {
                return new BaseResponse { Success = false, Message = "用户没有权限修改模块的配置数据" };
            }
            var data = await _tma.IsExistCheck(a => a.Id == ModuleId);
            if (!data.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的模块编号不存在" };
            }
            if (GroupId != data.GroupId && Code != _config["Group"])
            {
                return Unauthorized("用户没有权限");
            }
            var ret = await _tma.UpdateModuleArgumentAsync(Account, ModuleId, data.TypeId, req);
            return ret;
        }
        /// <summary>
        /// 删除模块配置数据
        /// </summary>
        /// <param name="ModuleId">模块标识</param>
        /// <param name="Id">配置数据标识</param>
        /// <returns>返回删除是否成功</returns>
        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteTypeModuleArgumentAsync(int ModuleId, int Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //只有该组织的管理员和超级管理员有权限
            if (!isAdmin)
            {
                return new BaseResponse { Success = false, Message = "用户没有权限修改模块的配置数据" };
            }
            var data = await _tma.IsExistCheck(a => a.Id == ModuleId);
            if (!data.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的模块编号不存在" };
            }
            if (GroupId != data.GroupId && Code != _config["Group"])
            {
                return Unauthorized("用户没有权限");
            }
            var ret = await _tma.DeleteTypeModuleArgumentAsync(Account, Id);
            return ret;
        }
        /// <summary>
        /// 获取模块的配置数据
        /// </summary>
        /// <param name="ModuleId">模块标识</param>
        /// <returns>返回模块配置的数据</returns>
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetTypeModuleArguments(int ModuleId)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //同一个组织的合法用户都可以查看
            var data = await _tma.IsExistCheck(a => a.Id == ModuleId);
            if (!data.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的模块编号不存在" };
            }
            if (GroupId != data.GroupId && !(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限");
            }
            var ret = await _tma.GetTypeModuleArgumentAsync(ModuleId);
            return ret;
        }
    }
}
