using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class RepairDataController : ControllerBase
    {
        private readonly IRepairService _repair;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepairDataService _repairData;
        private readonly IUserService _user;

        public RepairDataController(IRepairService repair, IConfiguration config, IWebHostEnvironment webHostEnvironment, IRepairDataService repairData, IUserService user)
        {
            this._repair = repair;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
            this._repairData = repairData;
            this._user = user;
        }
        /// <summary>
        /// 接收运维单
        /// </summary>
        /// <param name="Id">运维单编号</param>
        /// <returns></returns>
        [HttpPost("Receive/{Id}")]
        public async Task<BaseResponse> ReceiveRepairAsync(string Id)
        {
            var data = await _repair.IsExistAsync(a => a.Id == Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单不存在" };
            }
            if (data.RepairStatus != Model.RepairStatus.Send)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单已接单，不能重复接单" };
            }
            //检查用户是否是接单员,隐含接单人员已有接单权限（派单时已检查）
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (Account != data.Receiver)
            {
                return new BaseResponse { Success = false, Message = "用户和接单用户不一致" };
            }

            //获取用户中文名
            var u = await _user.GetUserByAccountAsync(Account);
            var ret = await _repairData.ReceiveAsync(u.UserName, Account, new AddRepairDataBaseDto { RepairId = Id });
            return ret;
        }
        /// <summary>
        /// 等待第三方维修
        /// </summary>
        /// <param name="req">运维单编号和备注信息</param>
        /// <returns></returns>
        [HttpPost("Third")]
        public async Task<BaseResponse> ThirdRepairAsync(AddRepairDataMessageDto req)
        {
            var data = await _repair.IsExistAsync(a => a.Id == req.RepairId);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单不存在" };
            }
            if (data.RepairStatus != Model.RepairStatus.Way)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单状态不是运维在途，不能设置为第三方处理" };
            }
            //检查用户是否是接单员,隐含接单人员已有接单权限（派单时已检查）
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (Account != data.Receiver)
            {
                return new BaseResponse { Success = false, Message = "用户和接单用户不一致" };
            }

            //获取用户中文名
            var u = await _user.GetUserByAccountAsync(Account);
            var ret = await _repairData.ThirdPartAsync(u.UserName, Account, req);
            return ret;
        }
        [HttpPost("Wait")]
        public async Task<BaseResponse> WaitRepairAsync(AddRepairDataMessageDto req)
        {
            var data = await _repair.IsExistAsync(a => a.Id == req.RepairId);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单不存在" };
            }
            if (data.RepairStatus != Model.RepairStatus.Way)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单状态不是运维在途，不能设置为等待配件" };
            }
            //检查用户是否是接单员,隐含接单人员已有接单权限（派单时已检查）
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (Account != data.Receiver)
            {
                return new BaseResponse { Success = false, Message = "用户和接单用户不一致" };
            }

            //获取用户中文名
            var u = await _user.GetUserByAccountAsync(Account);
            var ret = await _repairData.WaitAsync(u.UserName, Account, req);
            return ret;
        }
        [HttpPost("Upload")]
        public async Task<BaseResponse> UploadRepairAsync([FromForm] RepairDataAddImageRequest req)
        {
            var data = await _repair.IsExistAsync(a => a.Id == req.RepairId);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单不存在" };
            }
            //只能是已接单或者等待配件状态下才能处理
            if (!((int)data.RepairStatus == 1 || (int)data.RepairStatus == 2 || (int)data.RepairStatus == 3))
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
            AddRepairDataDto dto = new AddRepairDataDto();
            dto.RepairId = req.RepairId;
            dto.Message = req.Message;
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
                //获取用户中文名
                var u = await _user.GetUserByAccountAsync(Account);
                var ret = await _repairData.UploadAsync(u.UserName, Account, dto);
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
                return new BaseResponse { Success = false, Message = "上传运维数据出错，请联系管理员" };
            }
        }
        [HttpPost("Check")]
        public async Task<BaseResponse> CheckRepairAsync([FromBody] AddRepairCheckRequest req)
        {
            var data = await _repair.IsExistAsync(a => a.Id == req.RepairId);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单不存在" };
            }
            if (data.RepairStatus != Model.RepairStatus.Check)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单状态不是等待审核，不能审核" };
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
            //获取用户中文名
            var u = await _user.GetUserByAccountAsync(Account);
            AddRepairCheckDto dto = new AddRepairCheckDto() { RepairId = req.RepairId, Message = req.Message };
            var ret = await _repairData.CheckAsync(u.UserName, Account, req.Check, dto);
            return ret;
        }
    }
}
