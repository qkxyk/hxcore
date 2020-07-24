using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceImageUpdateDto
    {
        [Required(ErrorMessage = "图片标示不能为空")]
        public int Id { get; set; }
        [Required(ErrorMessage = "图片名称不能为空")]
        public string ImageName { get; set; }
        //public string Url { get; set; }
        [Range(1, 5, ErrorMessage = "图片排序只支持5种类型")]
        public int Rank { get; set; } = 1;//图片顺序
    }
}
