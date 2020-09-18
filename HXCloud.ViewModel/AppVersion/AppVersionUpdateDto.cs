using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class AppVersionUpdateDto
    {
        [Required(ErrorMessage = "升级文件的标示不能为空")]
        public int Id { get; set; }
        [Required(ErrorMessage = "升级文件的版本号必须输入")]
        public string VersionNo { get; set; }
        public string Description { get; set; }
        public bool State { get; set; } = false;//是否强制升级
        public int Type { get; set; } = 0;//升级文件类型
    }
}
