using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeAccessoryAddViewModel
    {
        [Required(ErrorMessage = "类型配件名称不能为空")]
        [StringLength(20, ErrorMessage = "类型配件名称长度在2到20个字符之间", MinimumLength = 2)]
        public string Name { get; set; }
        public string ICON { get; set; }//设备配件图标
    }
}
