using HXCloud.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ModuleOperateController : ControllerBase
    {
        private readonly IModuleOperateService _moduleOperate;

        public ModuleOperateController(IModuleOperateService moduleOperate)
        {
            this._moduleOperate = moduleOperate;
        }

    }
}
