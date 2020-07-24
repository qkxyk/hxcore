using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> _log;
        private readonly IDeviceService _ds;
        private readonly IProjectService _ps;
        private readonly ITypeService _ts;
        private readonly IGroupService _gs;
        private readonly IConfiguration _config;
        private readonly IRoleProjectService _rp;

        //获取设备时附带设备的图片、设备工艺图等信息,设备列表只返回设备的基本信息

        //注入项目用来验证项目是否存在，角色项目验证是否有权限操作，类型用来验证类型信息，组织用来验证组织信息
        public DeviceController(ILogger<DeviceController> log, IDeviceService ds, IProjectService ps, ITypeService ts, IGroupService gs, IConfiguration config, IRoleProjectService rp)
        {
            this._log = log;
            this._ds = ds;
            this._ps = ps;
            this._ts = ts;
            this._gs = gs;
            this._config = config;
            this._rp = rp;
        }
        //添加设备需要导入类型的硬件配置数据
        public async Task<ActionResult<BaseResponse>> AddDevice(DeviceAddDto req)
        {
            BaseResponse br = new BaseResponse();
            bool bRet = await _gs.IsExist(opt => opt.Id == req.GroupId);
            if (!bRet)
            {
                return new BaseResponse { Success = false, Message = "输入的组织编号不存在，请确认" };
            }
            bRet = await _ts.IsExist(opt => opt.Id == req.TypeId);
            if (!bRet)
            {
                return new BaseResponse { Success = false, Message = "输入的类型不存在，请确认" };
            }

            string user = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(user))
            {
                return Unauthorized("用户凭证缺失");
            }
            UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            if (req.ProjectId.HasValue && req.ProjectId.Value != 0)
            {
                string pathId, groupId;
                bRet = _ps.IsExist(req.ProjectId.Value, out pathId, out groupId);
                if (!bRet || req.GroupId != groupId)
                {
                    return new BaseResponse { Success = false, Message = "输入的项目不存在或者项目与组织编号不匹配" };
                }
                if (pathId == null || "" == pathId)
                {
                    pathId = req.ProjectId.Value.ToString();
                }
                if (um.IsAdmin)
                {
                    if (um.GroupId != req.GroupId && um.Code != _config["Group"])
                    {
                        return new BaseResponse { Success = false, Message = "用户所属的组织编号和该组织编号不匹配" };
                    }
                }
                else
                {
                    bRet = await _rp.IsAuth(um.Roles, pathId, 3);
                    if (!bRet)
                    {
                        return new BaseResponse { Success = false, Message = "用户没有在该场站下添加设备的权限" };
                    }
                }
            }
            else//无项目的设备，只有管理员才有权限添加
            {
                if (!(um.IsAdmin && (um.GroupId == req.GroupId || um.Code == _config["Group"])))
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限添加无项目的设备" };
                }
            }

            //var ret = _ds.

            throw new NotImplementedException();
        }
    }
}