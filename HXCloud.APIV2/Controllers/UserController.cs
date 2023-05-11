using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HXCloud.APIV2.Filters;
using HXCloud.Common;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//using log4net.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace HXCloud.APIV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger _log;
        private readonly IUserService _us;
        private readonly IGroupService _gs;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOptions<JwtConfig> _options;

        public UserController(ILogger<UserController> log, IUserService us, IGroupService gs, IConfiguration config, IWebHostEnvironment webHostEnvironment, IOptions<JwtConfig> options)
        {
            this._log = log;
            this._us = us;
            this._gs = gs;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
            this._options = options;
        }
        [HttpGet("MyInfo")]
        public async Task<ActionResult<BaseResponse>> MyInfo()
        {
            var Id = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var rm = await _us.GetUserInfoAsync(Id, isAdmin);
            return rm;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> Get(int Id)
        {
            var UId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            if (UId != Id)
            {
                //该组织管理员有权限
                var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
                var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
                var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
                var GId = await _us.GetUserGroupAsync(Id);
                if (!(isAdmin && (GroupId == GId || Code == _config["Group"])))
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限" };
                }
            }
            var rm = await _us.GetUserAsync(Id);
            return rm;
        }
        /// <summary>
        /// 获取组织所有的用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> Get([FromQuery] UserPageRequestViewModel req)
        {
            //该组织管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            if (!isAdmin)
            {
                return new UnauthorizedResult();
            }
            var rm = await _us.GetUsersAsync(req, GroupId);
            return rm;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Login(LoginViewModel req)
        {
            //1、验证用户名和密码是否匹配。2、验证用户是否有效用户。3、验证用户是否已分配组织。4、验证用户是否已分配角色
            #region 删除自定义的认证方式
            /*
                        var ret = await _us.UserLoginAsync(req);
                        if (ret.Success)
                        {
                            _log.LogInformation($"{req.Account}登录成功");
                        }
                        else
                        {
                            _log.LogInformation($"{req.Account}登录失败，失败原因:{ret.Message}");
                        }

                        return ret;
                        */
            #endregion
            #region 创建jwt认证
            //获取用户信息
            var login = await _us.UserLoginJwtAsync(req);
            if (!login.Success)
            {
                return login;
            }
            UserLoginDto u = login as UserLoginDto;
            var claims = new Claim[]
            {
                new Claim ("Account", u.Account),
                new Claim ("Role", u.Roles),
                new Claim("Code",u.Code),
                new Claim("GroupId",u.GroupId),
                new Claim("IsAdmin",u.IsAdmin.ToString()),
                new Claim("Id",u.Id.ToString()),
                new Claim("Key",$"{u.Account}+{u.Id}"),
                new Claim("Category",u.Category.ToString())
            };
            //ClaimsIdentity id =  
            var now = DateTime.Now;
            var expires = now.Add(TimeSpan.FromMinutes(_options.Value.AccessTokenExpiresMinutes));
            var token = new JwtSecurityToken(
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                claims: claims,
                //notBefore: now,
                //expires: expires,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.IssuerSigningKey)), SecurityAlgorithms.HmacSha256));
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            #endregion
            return new UserloginResponse { Success = true, Message = "用户登录成功", Token = jwtToken };
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<BaseResponse>> Register([FromBody] UserRegisterViewModel req)
        {
            BaseResponse br = new BaseResponse();
            if (req.Password != req.PasswordAgain)
            {
                br.Success = false;
                br.Message = "两次输入的密码不一致，请确认";
                return br;
            }
            bool bGroup = await _gs.IsExist(a => a.Id == req.GroupId);
            if (!bGroup)
            {
                #region 添加modelstate
                /*
                var err = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
                err.AddModelError("Message", "输入的组织编号不存在，请确认");
                return BadRequest(err);
                */
                #endregion
                //return BadRequest("输入的组织编号不存在，请确认");
                br.Success = false;
                br.Message = "输入的组织编号不正确";
                return br;
            }
            //验证是否重名（所有用户名都不能重复，不同组织也不允许重复）
            bool bUser = await _us.IsExist(a => a.Account == req.Account);
            if (bUser)
            {
                br.Success = false;
                br.Message = "此用户名已存在，请选择其他的用户名";
                //return br;
            }
            else
            {
                br = await _us.UserRegisterAsync(req);
            }
            return br;
        }

        [HttpPut]
        public async Task<ActionResult<BaseResponse>> UpdateUser(UserUpdateViewModel req)
        {
            //只有用户自己能更改用户的信息
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            var Id = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            return await _us.UpdateUserInfoAsync(Account, req, Id);
        }
        [HttpPut("Ops")]
        public async Task<ActionResult<BaseResponse>> UpdateUserOpsAsync([FromBody]UserOpsUpdateDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //该组织管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            //获取用户所在的组标示
            string GId = await _us.GetUserGroupAsync(req.Id);
            if (!(isAdmin && (GroupId == GId || Code == _config["Group"])))
            {
                return new BaseResponse { Success = false, Message = "用户没有权限" };
            }
            var ret = await _us.UpdateUserOpsAsync(Account, req);
            return ret;
        }

        [RequestSizeLimit(1024 * 1024 * 2)]
        [HttpPost("Picture")]
        public async Task<ActionResult<BaseResponse>> UpdateUserPicture([FromForm] IFormFile file/*, UserPictureViewModel req*/)
        {
            //图片保存为用户的id编号
            BaseResponse br = new BaseResponse();
            if (file == null)
            {
                br.Success = false;
                br.Message = "输入图像不能为空";
                return br;
            }
            //文件后缀
            var fileExtension = Path.GetExtension(file.FileName);
            //判断后缀是否是图片
            const string fileFilt = ".gif|.jpg|.jpeg|.png";
            if (fileExtension == null)
            {
                br.Success = false;
                br.Message = "上传的文件没有后缀";
                return br;
            }
            if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
            {
                br.Success = false;
                br.Message = "请上传jpg、png、gif格式的图片";
                return br;
            }
            //判断文件大小    
            long length = file.Length;
            if (length > 1024 * 1024 * 2) //2M
            {
                br.Success = false;
                br.Message = "上传的文件不能大于2M";
                return br;
            }
            var Id = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            //var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //var GId = await _us.GetUserGroupAsync(Id);

            ////只有用户自己能上传头像
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);

            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
                                                                 //string contentRootPath = _webHostEnvironment.ContentRootPath;//根目录
            string ext = Id + fileExtension;//头像名称修改为用户编号加后缀名
            string userPath = Path.Combine(GroupId, "User");//用户头像保存位置
            userPath = Path.Combine(_config["StoredImagesPath"], userPath);
            string path = Path.Combine(userPath, ext);//头像保存地址（相对路径）
            var filePath = Path.Combine(webRootPath, userPath);//物理路径,不包含头像名称
                                                               //如果路径不存在，创建路径
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            filePath = Path.Combine(filePath, ext);//头像的物理路径
            try
            {
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                br = await _us.UpdateUserImageAsync(Id, Account, path);
                if (!br.Success)
                {
                    //删除已存在的logo
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                //删除已存在的logo
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                br.Success = false;
                br.Message = "上传用户头像失败";
                _log.LogError($"{Account}上传用户头像失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
            }
            return br;
        }

        [HttpPut("Status")]
        public async Task<ActionResult<BaseResponse>> UpdateUserStatus(UserStatusUpDateViewModel req)
        {
            //该组织管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            //获取用户所在的组标示
            string GId = await _us.GetUserGroupAsync(req.Id);
            if (!(isAdmin && (GroupId == GId || Code == _config["Group"])))
            {
                return new BaseResponse { Success = false, Message = "用户没有权限" };
            }
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _us.UpdateUserStatusAsync(req, Account);
            return rm;
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteUser(int Id)
        {
            //该组织管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            //获取用户所在的组标示
            string GId = await _us.GetUserGroupAsync(Id);
            if (!(isAdmin && (GroupId == GId || Code == _config["Group"])))
            {
                return new BaseResponse { Success = false, Message = "用户没有权限" };
            }
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _us.DeleteUserAsync(Id, Account);
            return rm;
        }

        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="req">密码信息</param>
        /// <returns></returns>
        [HttpPost("Password")]
        public async Task<ActionResult<BaseResponse>> Password(UserPasswordViewModel req)
        {
            BaseResponse br = new BaseResponse();
            if (req.Password != req.PasswordAgain)
            {
                br.Success = false;
                br.Message = "两次输入的密码不一致";
                return br;
            }
            var Id = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            br = await _us.UpdateUserPasswordAsync(req, Id);
            return br;
        }
        [HttpPut("Password")]
        public async Task<ActionResult<BaseResponse>> ResetPassword(UserResetPasswordViewModel req)
        {
            //该组织管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            //获取用户所在的组标示
            string GId = await _us.GetUserGroupAsync(req.Id);
            if (!(isAdmin && (GroupId == GId || Code == _config["Group"])))
            {
                return new BaseResponse { Success = false, Message = "用户没有权限" };
            }
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _us.ResetPassword(req, Account);
            return rm;
        }

        //[TypeFilter(typeof(AdminActionFilterAttribute))]
        [HttpPost("AddUser")]
        public async Task<ActionResult<BaseResponse>> AddUser([FromBody] UserAddViewModel req)
        {
            //该组织管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (req.Password != req.PasswordAgain)
            {
                return new BaseResponse { Success = false, Message = "两次输入的密码不一致" };
            }
            if (!isAdmin)
            {
                return new BaseResponse { Success = false, Message = "用户没有权限" };
            }
            var rm = await _us.AddUserAsync(Account, GroupId, req);
            return rm;
        }

        /// <summary>
        /// 根据运维经理账户名获取用户下级运维用户
        /// </summary>
        [HttpGet("OpsUser")]
        public async Task<ActionResult<BaseResponse>> GetOpsUsers()
        {
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //验证用户是否运维账号
            var user = await _us.GetUserByAccountAsync(Account);
            if (user==null)
            {
                return new BaseResponse { Success = false, Message = "用户不存在" };
            }
            if (user.Category<3)
            {
                return new BaseResponse { Success = false, Message = "只有运维经理有权限获取运维人员信息" };
            }
            var data = await _us.GetOpsUserAsync(Account);
            return new BResponse<List<OpsUserDto>> { Success = true, Message = "获取数据成功", Data = data };
        }

        [HttpGet("OpsName")]
        public async Task<List<string>> GetUserNameAsync()
        {
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var ret = await _us.GetUserAndChildNameAsync(Account,isAdmin);
            return ret;
        }
    }
}
