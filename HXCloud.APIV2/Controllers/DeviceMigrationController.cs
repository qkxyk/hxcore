using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/{GroupId}/{DeviceSn}/[controller]")]
    [ApiController]
    public class DeviceMigrationController : ControllerBase
    {
        private readonly IDeviceService _ds;
        private readonly IDeviceMigrationService _dms;

        public DeviceMigrationController(IDeviceService ds, IDeviceMigrationService dms)
        {
            this._ds = ds;
            this._dms = dms;
        }
        /// <summary>
        /// 获取设备的迁移记录，只有管理员有权限查看设备的迁移记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetMigrations(string GroupId, string DeviceSn)
        {
            var device = await _ds.IsExistCheck(a => a.DeviceSn == DeviceSn);
            if (!device.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备编号不存在" };
            }
            var rm = await _dms.GetDeviceMigrationAsync(DeviceSn);
            return rm;
        }
    }
}