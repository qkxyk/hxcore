using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeUpdateViewModel
    {
        [Required(ErrorMessage ="类型标示不能为空")]
        public int Id { get; set; }
        [Required(ErrorMessage ="类型名称不能为空")]
        [StringLength(100, ErrorMessage = "类型名称长度在2到100个字符之间", MinimumLength = 2)]
        public string TypeName { get; set; }//设备类型名称
        public string Description { get; set; }//设备类型描述
        public string ICON { get; set; }//类型图标名称（后台只存图标名称,图标放在客户端）2019-1-10添加
    }
}
