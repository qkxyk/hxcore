using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class CraftComponentCatalogEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
    public class CraftComponentCatalogEditRequest
    {
        [Required(ErrorMessage = "分类名称不能为空")]
        [StringLength(25, ErrorMessage = "分类名称长度在2到25个字符之间", MinimumLength = 2)]
        public string Name { get; set; }
        [Required(ErrorMessage = "图片不能为空")]
        public IFormFile file { get; set; }
    }
}
