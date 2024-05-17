using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/{typeId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeCraftTopController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ITypeCraftTopService _typeCraftTop;
        private readonly IUserService _user;
        private readonly ILogger<TypeCraftTopController> _log;
        private readonly ITypeService _type;

        public TypeCraftTopController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ITypeCraftTopService typeCraftTop,
            IUserService user, ILogger<TypeCraftTopController> log,ITypeService type)
        {
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
            this._typeCraftTop = typeCraftTop;
            this._user = user;
            this._log = log;
            this._type = type;
        }
        [HttpPost]
        public async Task<BaseResponse> AddCraftTopAsync(int typeId, [FromBody] TypeCraftTopAddRequest req)
        {
            var isType = await _type.IsExist(a => a.Id == typeId);
            if (!isType)
            {
                return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            }
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string fileExtension = ".top";
            //类型图片保存的相对路径：Image+组织编号+TypeImage+TypeId+图片名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            //string contentRootPath = _webHostEnvironment.ContentRootPath;//根目录
            string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;//保存的文件名
            string userPath = Path.Combine(GroupId, "TypeImage", typeId.ToString());
            userPath = Path.Combine(_config["StoredImagesPath"], userPath);//文件保存路径
            string path = Path.Combine(userPath, ext);//文件保存地址（相对路径）
            var filePath = Path.Combine(webRootPath, userPath);//物理路径,不包含文件名称
            //如果路径不存在，创建路径
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            filePath = Path.Combine(filePath, ext);//文件的物理路径，包含文件名称
            try
            {
                using (StreamWriter sw = System.IO.File.CreateText(filePath))
                {
                    await sw.WriteAsync(req.Data);
                }
                TypeCraftTopAddDto dto = new TypeCraftTopAddDto
                {
                    Key = req.Key,
                    Name = req.Name,
                    Sn = req.Sn,
                    TypeId = typeId,
                    Url = path
                };
                var data = await _typeCraftTop.AddTypeCraftTopAsync(Account, dto);
                return data;
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加类型拓扑数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse
                {
                    Success = false,
                    Message = "添加数据失败，请联系管理员"
                };
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetTypeCraftTopById(int Id)
        {
            var data = await _typeCraftTop.GetTypeCraftTopByIdAsync(Id);
            return data;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetTypeCraftTopAsync(int TypeId)
        {
            var data = await _typeCraftTop.GetTypeCraftTopAsync(TypeId);
            return data;
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteTypeCraftTopAsync(int Id)
        {
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var UserId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            //类型图片保存的相对路径：Image+组织编号+TypeImage+TypeId+图片名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var isAdmin = await _user.IsAdminAsync(UserId);
            var data = await _typeCraftTop.DeleteTypeCraftTopAsync(Id, Account, isAdmin, webRootPath);
            return data;
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<BaseResponse>> EditTypeCraftTopAsync(int Id, int typeId, [FromBody] TypeCraftTopEditRequest req)
        {
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var isExist = await _typeCraftTop.IsCraftTopExist(a => a.Id == Id);
            if (!isExist.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的数据不存在" };
            }
            if (Account!=isExist.Account)
            {
                return new BaseResponse { Success = false, Message = "用户没有权限编辑该数据" };
            }
            TypeCraftTopEditDto dto = new TypeCraftTopEditDto();
            dto.Key = req.Key;
            dto.Name = req.Name;
            dto.Sn = req.Sn;
            dto.TypeId = typeId;
            dto.Id = Id;
            if (string.IsNullOrEmpty(req.Data))//不更改data数据
            {
                dto.Url = isExist.Url;
            }
            else
            {
                try
                {
                    var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
                    string fileExtension = ".top";
                    //类型图片保存的相对路径：Image+组织编号+TypeImage+TypeId+图片名称
                    string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
                                                                         //先删除原有的文件
                    string oldUrl = Path.Combine(webRootPath, isExist.Url);
                    if (System.IO.File.Exists(oldUrl))
                    {
                        System.IO.File.Delete(oldUrl);
                    }
                    //string contentRootPath = _webHostEnvironment.ContentRootPath;//根目录
                    string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;//保存的文件名
                    string userPath = Path.Combine(GroupId, "TypeImage", typeId.ToString());
                    userPath = Path.Combine(_config["StoredImagesPath"], userPath);//文件保存路径
                    string path = Path.Combine(userPath, ext);//文件保存地址（相对路径）
                    var filePath = Path.Combine(webRootPath, userPath);//物理路径,不包含文件名称
                                                                       //如果路径不存在，创建路径
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                    filePath = Path.Combine(filePath, ext);//文件的物理路径，包含文件名称

                    using (StreamWriter sw = System.IO.File.CreateText(filePath))
                    {
                        await sw.WriteAsync(req.Data);
                    }
                    dto.Url = path;
                }
                catch (Exception ex)
                {
                    _log.LogError($"{Account}修改标识为{Id}的类型拓扑数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "修改数据失败，请联系管理员"
                    };
                }
            }
            var data = await _typeCraftTop.UpdateTypeCraftTopAsync(Account, dto);
            return data;
        }
    }
}
