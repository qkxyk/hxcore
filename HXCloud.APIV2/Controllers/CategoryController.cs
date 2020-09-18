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

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _cs;

        public CategoryController(ICategoryService cs)
        {
            this._cs = cs;
        }
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddCategory([FromBody]CategoryAddDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _cs.AddCategroyAsync(Account, req);
            return rm;
        }
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> UpdateCategory([FromBody]CategoryUpdateDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _cs.UpdateCategoryAsync(Account, req);
            return rm;
        }
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteCategory(int Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _cs.DeleteCategoryAsync(Account, Id);
            return rm;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetAll()
        {
            var data = await _cs.GetCategroyAsync();
            return data;
        }
    }
}