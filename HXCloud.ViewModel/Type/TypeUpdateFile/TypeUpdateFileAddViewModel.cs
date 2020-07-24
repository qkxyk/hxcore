using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HXCloud.ViewModel
{
    public class TypeUpdateFileAddViewModel
    {
        [Required(ErrorMessage = "升级文件的名称必须输入")]
        public string Name { get; set; }
        [Required(ErrorMessage = "升级文件的版本号必须输入")]
        public string Version { get; set; }

        public string Description { get; set; }
        [Required(ErrorMessage = "类型更新文件不能为空")]
        public IFormFile file { get; set; }
    }
}
