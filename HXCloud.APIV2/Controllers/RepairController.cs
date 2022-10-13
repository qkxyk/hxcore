using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RepairController : ControllerBase
    {
        private readonly ILogger<RepairController> _log;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepairService _repair;
        private readonly IIssueService _issue;
        private readonly IUserService _user;
        private readonly IDeviceService _device;
        private readonly IRoleProjectService _rps;

        public RepairController(ILogger<RepairController> log, IConfiguration config, IWebHostEnvironment webHostEnvironment, IRepairService repair, IIssueService issue, IUserService user, IDeviceService device, IRoleProjectService rps)
        {
            this._log = log;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
            this._repair = repair;
            this._issue = issue;
            this._user = user;
            this._device = device;
            this._rps = rps;
        }

        /// <summary>
        /// 派单,运维管理者创建运维单
        /// </summary>
        /// <param name="req">运维单数据</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddRepairAsync([FromBody] RepairAddRequest req)
        {
            RepairAddDto dto = new RepairAddDto();
            if (req.IssueId.HasValue)
            {
                var issueData = await _issue.IsExist(a => a.Id == req.IssueId.Value);
                if (!issueData)
                {
                    return new BaseResponse { Success = false, Message = "输入的问题单不存在" };
                }
                dto.IssueId = req.IssueId.Value;
            }
            var userData = await _user.GetUserByAccountAsync(req.Receiver);
            if (userData == null)
            {
                return new BaseResponse { Success = false, Message = "输入的接收者不存在" };
            }
            if (string.IsNullOrEmpty(userData.Roles))
            {
                return new BaseResponse { Success = false, Message = "接收人没有分配角色，不能分配" };
            }
            var deviceData = await _device.CheckDeviceAsync(req.DeviceSn);
            if (deviceData == null)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在，请确认" };
            }
            if (string.IsNullOrEmpty(deviceData.PathId))
            {
                return new BaseResponse { Success = false, Message = "该设备没有分配场站，不能操作" };
            }
            bool bAuthUser = await _rps.IsAuth(userData.Roles, deviceData.PathId, 0);
            if (!bAuthUser)
            {
                return new BaseResponse { Success = false, Message = "接收人对该设备没有权限" };
            }
            dto.Receiver = req.Receiver;
            dto.ReceivePhone = userData.Phone;
            dto.RepairType = req.RepairType;
            dto.EmergenceStatus = req.EmergenceStatus;
            dto.DeviceSn = req.DeviceSn;
            dto.DeviceName = deviceData.DeviceName;
            dto.Description = req.Description;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            int Id = 0;
            int.TryParse(User.Claims.FirstOrDefault(a => a.Type == "Id").Value, out Id);
            //验证用户对设备是否有权限，并且用户是否是运维管理者
            //运维人员
            var ops = User.Claims.FirstOrDefault(a => a.Type == "Category").Value;
            int category = 0;
            int.TryParse(ops, out category);
            //管理员和运维人员可以通过
            if (!IsAdmin && category < 3)
            {
                return new BaseResponse { Message = "用户没有权限审核运维单", Success = false };
            }
            //验证用户是否对该运维单有权限
            //检测关联的设备是否存在
            //检查是否有设备查看权限
            if (!IsAdmin)        //非管理员验证权限
            {
                //是否有设备的查看权限
                bool bAuth = await _rps.IsAuth(Roles, deviceData.PathId, 0);
                if (!bAuth)
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限查看设备的功能" };
                }
            }
            var ret = await _repair.AddRepairAsync(Account, Id, dto);
            return ret;
        }
        /// <summary>
        /// 接单
        /// </summary>
        /// <param name="req">运维单编号</param>
        /// <returns></returns>
        [HttpPut("Receive")]
        public async Task<ActionResult<BaseResponse>> ReceiveRepairAsync([FromBody] RepairStatusUpdateDto req)
        {
            var data = await _repair.IsExistAsync(a => a.Id == req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单不存在" };
            }
            if (req.RepairStatus != 1)
            {
                return new BaseResponse { Success = false, Message = "接单状态只能为1" };
            }
            if ((int)data.RepairStatus != 0)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单已接单，不能重复接单" };
            }
            //检查用户是否是接单员,隐含接单人员已有接单权限（派单时已检查）
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (Account != data.Receiver)
            {
                return new BaseResponse { Success = false, Message = "用户和接单用户不一致" };
            }
            var ret = await _repair.ReceiveRepairAsync(req.Id, Account, req.RepairStatus);
            return ret;
        }
        /// <summary>
        /// 设置运维单为等待配件
        /// </summary>
        /// <param name="req">运维单状态信息</param>
        /// <returns></returns>
        [HttpPut("Wait")]
        public async Task<ActionResult<BaseResponse>> WaitRepairAsync(RepairStatusUpdateDto req)
        {
            var data = await _repair.IsExistAsync(a => a.Id == req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单不存在" };
            }
            if (req.RepairStatus != 2)
            {
                return new BaseResponse { Success = false, Message = "等待配件状态为2" };
            }
            if ((int)data.RepairStatus != 1)
            {
                return new BaseResponse { Success = false, Message = "运维单没有接单或者状态不正确，请联系管理员" };
            }
            //检查用户是否是接单员,隐含接单人员已有接单权限（派单时已检查）
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (Account != data.Receiver)
            {
                return new BaseResponse { Success = false, Message = "用户和接单用户不一致" };
            }
            var ret = await _repair.WaitRepairAsync(req.Id, Account, req.RepairStatus);
            return ret;
        }

        /// <summary>
        /// 上传运维结果
        /// </summary>
        /// <param name="req">运维结果</param>
        /// <returns></returns>
        [HttpPost("UpLoad")]
        public async Task<ActionResult<BaseResponse>> UpLoadRepairImageAsync([FromForm] RepairAddImageRequest req)
        {
            var data = await _repair.IsExistAsync(a => a.Id == req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单不存在" };
            }
            /*   if ((int)data.RepairStatus != 1)
               {
                   return new BaseResponse { Success = false, Message = "运维单没有接单或者状态不正确，请联系管理员" };
               }
               if ((int)data.RepairStatus == 4)
               {
                   return new BaseResponse { Success = false, Message = "该运维单已结束" };
               }*/
            //只能是已接单或者等待配件状态下才能处理
            if (!((int)data.RepairStatus == 1 || (int)data.RepairStatus == 2))
            {
                return new BaseResponse { Success = false, Message = "运维单没有接单或者状态不正确，请联系管理员" };
            }
            //检测图片类型和图片大小
            foreach (var item in req.file)
            {
                //文件后缀
                var fileExtension = Path.GetExtension(item.FileName);
                //判断后缀是否是图片
                const string fileFilt = ".gif|.jpg|.jpeg|.png";
                if (fileExtension == null)
                {
                    return new BResponse<string> { Success = false, Message = "上传的文件没有后缀", Data = $"{item.FileName}没有后缀名" };
                }
                if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
                {
                    return new BResponse<string> { Success = false, Message = "请上传jpg、png、gif格式的图片", Data = $"{item.FileName}格式不正确" };
                }
                //判断文件大小    
                long length = item.Length;
                if (length > 1024 * 1024 * 5) //5M
                {
                    return new BResponse<string> { Success = false, Message = "上传的图片不能大于5M", Data = $"{item.FileName}太大" };
                }
            }
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            RepairAddImageDto dto = new RepairAddImageDto();
            dto.Id = req.Id;
            dto.Description = req.Description;
            //图片保存的相对路径：image+组织编号+ops+图片名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            string userPath = Path.Combine(GroupId, "Ops", "RepairImage");//保存位置    
            userPath = Path.Combine(_config["StoredImagesPath"], userPath);
            var filePath = Path.Combine(webRootPath, userPath);//物理路径,不包含文件名称
            //如果路径不存在，创建路径
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            List<string> imageUrl = new List<string>();
            try
            {
                foreach (var formFile in req.file)
                {
                    if (formFile.Length > 0)
                    {
                        //var fileExtension = Path.GetExtension(formFile.FileName);
                        string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + formFile.FileName;//图片名称修改为日期加图片名称
                        var imagePath = Path.Combine(filePath, ext);//文件的物理路径
                        imageUrl.Add(Path.Combine(userPath, ext));//图片的相对保存路径
                        using (var stream = System.IO.File.Create(imagePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }
                //文件传入数据库
                dto.Url = JsonConvert.SerializeObject(imageUrl);
                var ret = await _repair.UploadRepairImageAsync(Account, dto);
                if (!ret.Success)//删除已上传的文件
                {
                    //删除已上传的文件
                    foreach (var item in imageUrl)
                    {
                        string url = Path.Combine(filePath, item);
                        if (System.IO.File.Exists(url))
                        {
                            System.IO.File.Delete(url);
                        }
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                foreach (var item in imageUrl)
                {
                    string url = Path.Combine(filePath, item);
                    if (System.IO.File.Exists(url))
                    {
                        System.IO.File.Delete(url);
                    }

                }
                _log.LogError($"{Account}上传运维文件出错，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "上传运维数据出错，请联系管理员" };
            }
        }

        [HttpPut("Check")]
        public async Task<ActionResult<BaseResponse>> CheckRepairAsync([FromBody] RepairCheckDto req)
        {
            //验证运维单是否是核验状态
            var data = await _repair.IsExistAsync(a => a.Id == req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单不存在" };
            }
            if ((int)data.RepairStatus != 3)
            {
                return new BaseResponse { Success = false, Message = "运维单状态不是审核状态，无法审核" };
            }
            //只有项目管理者能审核,并且对设备有权限
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            //运维人员
            var ops = User.Claims.FirstOrDefault(a => a.Type == "Category").Value;
            int category = 0;
            int.TryParse(ops, out category);
            //管理员和运维人员可以通过
            if (!isAdmin && category < 3)
            {
                return new BaseResponse { Message = "用户没有权限审核运维单", Success = false };
            }
            //验证用户是否对该运维单有权限
            //检测关联的设备是否存在
            var device = await _device.CheckDeviceAsync(data.DeviceSn);
            if (!device.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在，请确认" };
            }
            //检查是否有设备查看权限
            if (!isAdmin)        //非管理员验证权限
            {
                //是否有设备的查看权限
                bool bAuth = await _rps.IsAuth(Roles, device.PathId, 0);
                if (!bAuth)
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限查看设备的功能" };
                }
            }
            var ret = await _repair.CheckRepairAsync(Account, req);
            return ret;
        }

        /// <summary>
        /// 删除运维单,只能删除为接单的运维单
        /// </summary>
        /// <param name="Id">运维单</param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteRepairAsync(string Id)
        {
            var data = await _repair.IsExistAsync(a => a.Id == Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单号不存在" };
            }
            //只有项目管理者能删除,并且对设备有权限
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            //运维人员
            var ops = User.Claims.FirstOrDefault(a => a.Type == "Category").Value;
            int category = 0;
            int.TryParse(ops, out category);
            //管理员和运维人员可以通过
            if (!isAdmin && category < 3)
            {
                return new BaseResponse { Message = "用户没有权限删除运维单", Success = false };
            }
            //验证用户是否对该运维单有权限
            //检测关联的设备是否存在
            var device = await _device.CheckDeviceAsync(data.DeviceSn);
            if (!device.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在，请确认" };
            }
            //检查是否有设备查看权限
            if (!isAdmin)        //非管理员验证权限
            {
                //是否有设备的查看权限
                bool bAuth = await _rps.IsAuth(Roles, device.PathId, 0);
                if (!bAuth)
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限查看设备的功能" };
                }
            }
            var ret = await _repair.DeleteRepairAsync(Account, Id);
            return ret;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetRepairByIdAsync(string Id)
        {
            //只有项目管理者能删除,并且对设备有权限
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            //var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            ////运维人员
            //var ops = User.Claims.FirstOrDefault(a => a.Type == "Category").Value;
            //int category = 0;
            //int.TryParse(ops, out category);
            ////管理员和运维人员可以通过
            //if (!isAdmin && category < 3)
            //{
            //    return new BaseResponse { Message = "用户没有权限删除运维单", Success = false };
            //}
            var ret = await _repair.GetRepairByIdAsync(Account, Id);
            return ret;
        }

        [HttpGet]
        public async Task<BaseResponse> GetRepairAsync([FromQuery] RepairRequest req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (string.IsNullOrWhiteSpace(req.Account))//如果不输入默认是看自己的
            {
                req.Account = Account;
            }
            else
            {
                var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
                var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
                var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
                var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
                //运维人员
                var ops = User.Claims.FirstOrDefault(a => a.Type == "Category").Value;
                int category = 0;
                int.TryParse(ops, out category);
                if (req.Account.Trim().ToLower() != Account.ToLower())
                {
                    //验证用户是否是该用户的上级
                    if (!isAdmin)
                    {
                        var users = await _user.GetUserAndChildAsync(Account, isAdmin);
                        if (!users.ContainsKey(req.Account))
                        {
                            return new BaseResponse { Success = false, Message = "用户没有查看该用户运维单的权限" };
                        }
                    }
                }
            }
            var ret = await _repair.GetRepairAsync(req);
            return ret;
        }
        [HttpGet("Page")]
        public async Task<BaseResponse> GetPageRepairAsync([FromQuery] RepairPageRequest req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (string.IsNullOrWhiteSpace(req.Account))//如果不输入默认是看自己的
            {
                req.Account = Account;
            }
            else
            {
                var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
                var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
                var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
                var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
                //运维人员
                var ops = User.Claims.FirstOrDefault(a => a.Type == "Category").Value;
                int category = 0;
                int.TryParse(ops, out category);
                if (req.Account.Trim().ToLower() != Account.ToLower())
                {
                    //验证用户是否是该用户的上级
                    if (!isAdmin)
                    {
                        var users = await _user.GetUserAndChildAsync(Account, isAdmin);
                        if (!users.ContainsKey(req.Account))
                        {
                            return new BaseResponse { Success = false, Message = "用户没有查看该用户运维单的权限" };
                        }
                    }
                }
            }
            var ret = await _repair.GetPageRepairAsync(req);
            return ret;
        }
    }
}
