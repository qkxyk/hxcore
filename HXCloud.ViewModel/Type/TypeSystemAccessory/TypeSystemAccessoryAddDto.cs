﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeSystemAccessoryAddDto
    {
        [Required(ErrorMessage = "类型配件名称不能为空")]
        [StringLength(50, ErrorMessage = "类型配件名称长度在2到50个字符之间")]
        public string Name { get; set; }
        public string ICON { get; set; }//设备配件图标
        //[Required(ErrorMessage = "类型子系统标示不能为空")]
        //public int SystemId { get; set; }
    }
}
