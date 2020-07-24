using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HXCloud.ViewModel
{
    public class TypeImageAddViewModel
    {
        public string ImageName { get; set; }
        //public string Url { get; set; }
        [Range(1, 5, ErrorMessage = "图片排序只支持5种类型")]
        public int Rank { get; set; } = 1;//图片顺序
        public string Description { get; set; }
        [Required(ErrorMessage = "图片不能为空")]
        public IFormFile file { get; set; }
        //[Required(ErrorMessage = "类型标示不能为空")]
        //public int TypeId { get; set; }
    }
}
