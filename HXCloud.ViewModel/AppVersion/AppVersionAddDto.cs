using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HXCloud.ViewModel
{
    public class AppVersionAddDto
    {
        public string Description { get; set; }
        [Required(ErrorMessage = "升级文件不能为空")]
        public IFormFile file { get; set; }
        [Required(ErrorMessage = "升级文件的版本号必须输入")]
        public string VersionNo { get; set; }
        public bool State { get; set; } = false;//是否强制升级
        public int Type { get; set; } = 0;//升级文件类型
    }
}
