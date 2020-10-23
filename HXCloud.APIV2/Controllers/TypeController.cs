using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// 类型支持添加、编辑类型名称、删除类型、根据类型标示获取类型信息（附带类型子类型）、根据组织标示回去类型、导入合续类型(暂未开发2020-4-13)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeController : ControllerBase
    {
        private readonly ILogger<TypeController> _log;
        private readonly ITypeService _ts;
        private readonly IGroupService _gs;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;

        public TypeController(ILogger<TypeController> log, IConfiguration config, ITypeService ts, IGroupService gs, IWebHostEnvironment webHostEnvironment)
        {
            this._log = log;
            this._ts = ts;
            this._gs = gs;
            this._webHostEnvironment = webHostEnvironment;
            this._config = config;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddType(TypeAddViewModel req)
        {
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //验证输入的groupid是否存在
            var ex = await _gs.IsExist(a => a.Id == req.GroupId);
            if (!ex)
            {
                return new BaseResponse { Success = false, Message = "输入的组织编号不存在" };
            }
            //验证用户权限
            if (!(isAdmin && (GroupId == req.GroupId || Code == _config["Group"])))
            {
                return Unauthorized("没有权限");
            }
            var ret = await _ts.AddType(req, Account);
            return ret;
        }
        /// <summary>
        /// 类型复制（分两步，第一步复制类型基本数据，第二步使用第一步返回的类型标示复制类型更新文件和工艺图），把
        /// 一个叶子节点类型复制到另一个非叶子节点下.只支持源类型类型为叶子节点的类型，目标类型为非叶子节点的类型
        /// </summary>
        /// <param name="GroupId">组织标示，从请求地址中获取</param>
        /// <param name="req">包含源类型标示和目标类型标示</param>
        /// <returns>把一个类型复制到另一个类型下</returns>
        [HttpPost("/api/{GroupId}/[controller]/Copy")]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> CopyTo(string GroupId, [FromBody]TypeCopyDto req)
        {
            //var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            //var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //验证输入的groupid是否存在
            var ex = await _gs.IsExist(a => a.Id == GroupId);
            if (!ex)
            {
                return new BaseResponse { Success = false, Message = "输入的组织编号不存在" };
            }
            //验证类型编号是否存在
            var source = await _ts.CheckTypeAsync(a => a.Id == req.SourceId && a.GroupId == GroupId);
            if (source.IsExist == false)
            {
                return new BaseResponse { Success = false, Message = "输入的源类型标示不存在" };
            }
            if (source.Status == 0)
            {
                return new BaseResponse { Success = false, Message = "源类型不能为目录节点" };
            }
            var target = await _ts.CheckTypeAsync(a => a.Id == req.TargetId && a.GroupId == GroupId);
            if (target.IsExist == false)
            {
                return new BaseResponse { Success = false, Message = "输入的目标类型标示不存在" };
            }
            if (target.Status == 1)
            {
                return new BaseResponse { Success = false, Message = "目标类型不能为叶子节点" };
            }

            ////验证用户权限
            //if (!(isAdmin && (GId == GroupId || Code == _config["Group"])))
            //{
            //    return Unauthorized("没有权限");
            //}
            var rm = await _ts.CopyTypeAsync(Account, req.SourceId, req.TargetId);
            return rm;
        }

        [HttpPost("/api/{GroupId}/[controller]/CopyFiles")]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> CopyFilesAsync(string GroupId, [FromBody]TypeCopyDto req)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //验证输入的groupid是否存在
            var ex = await _gs.IsExist(a => a.Id == GroupId);
            if (!ex)
            {
                return new BaseResponse { Success = false, Message = "输入的组织编号不存在" };
            }
            //验证类型编号是否存在
            var source = await _ts.CheckTypeAsync(a => a.Id == req.SourceId && a.GroupId == GroupId);
            if (source.IsExist == false)
            {
                return new BaseResponse { Success = false, Message = "输入的源类型标示不存在" };
            }
            if (source.Status == 0)
            {
                return new BaseResponse { Success = false, Message = "源类型不能为目录节点" };
            }
            var target = await _ts.CheckTypeAsync(a => a.Id == req.TargetId && a.GroupId == GroupId);
            if (target.IsExist == false)
            {
                return new BaseResponse { Success = false, Message = "输入的目标类型标示不存在" };
            }
            if (target.Status == 0)
            {
                return new BaseResponse { Success = false, Message = "目标类型不能为目录节点" };
            }
            //类型文件保存的相对路径：Files+组织编号+TypeFiles+TypeId+文件名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            string userPath = Path.Combine(GroupId, "TypeFiles", req.TargetId.ToString());//用户头像保存位置
            userPath = Path.Combine(_config["StoredFilesPath"], userPath);
            var filePath = Path.Combine(webRootPath, userPath);//物理路径
            //如果路径不存在，创建路径
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            //类型图片保存的相对路径：Files+组织编号+TypeImage+TypeId+文件名称
            string ImagePath = Path.Combine(GroupId, "TypeImage", req.TargetId.ToString());//用户头像保存位置
            ImagePath = Path.Combine(_config["StoredImagesPath"], ImagePath);
            ImagePath = Path.Combine(webRootPath, ImagePath);//物理路径,不包含头像名称
                                                             //如果路径不存在，创建路径
            if (!Directory.Exists(ImagePath))
                Directory.CreateDirectory(ImagePath);
            int fi = filePath.LastIndexOf('\\');
            int im = ImagePath.LastIndexOf('\\');
            filePath = filePath.Substring(0, fi);
            ImagePath = ImagePath.Substring(0, im);
            var rm = await _ts.CopyTypeFilesAsync(Account, filePath, ImagePath, req.SourceId, req.TargetId);
            return rm;
        }

        [HttpPut("{Id}")]
        [TypeFilter(typeof(TypeViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> Update(int Id, [FromBody]TypeUpdateViewModel req)
        {
            //var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _ts.UpdateType(req, Account);
            return ret;
        }

        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> Delete(int Id)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _ts.DeleteTypeAsync(Id, account);
            return ret;
        }

        [HttpGet("{Id}")]
        [TypeFilter(typeof(TypeViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> Get(int Id)
        {
            //此处待商榷（要现在用户只能访问自己所在组织的类型）
            var ret = await _ts.GetTypeAsync(Id);
            return ret;
        }

        [HttpGet]
        [TypeFilter(typeof(TypeViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetGroupType()
        {
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var ret = await _ts.GetGroupTypeAsync(GroupId);
            return ret;
        }


    }
}