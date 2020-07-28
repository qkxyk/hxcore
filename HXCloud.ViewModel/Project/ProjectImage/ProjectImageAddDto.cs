using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HXCloud.ViewModel
{
    public class ProjectImageAddDto
    {
        [Required(ErrorMessage = "图片名称必须输入")]
        public string ImageName { get; set; }
        [Required(ErrorMessage = "图片不能为空")]
        public IFormFile file { get; set; }
        public int Rank { get; set; } = 1;//图片顺序
    }
}
