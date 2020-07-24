using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HXCloud.ViewModel
{
    public class DeviceImageAddDto
    {
        [Required(ErrorMessage ="图片名称不能为空")]
        public string ImageName { get; set; }
        //public string Url { get; set; }
        [Range(1, 5, ErrorMessage = "图片排序只支持5种类型")]
        public int Rank { get; set; } = 1;//图片顺序
        [Required(ErrorMessage = "图片不能为空")]
        public IFormFile file { get; set; }
    }
}
