using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceMigDto
    {
        [Required(ErrorMessage = "设备序列号不能为空")]
        public string DeviceSn { get; set; }
        [Required(ErrorMessage = "必须输入要迁入的场站编号")]
        public int ProjectId { get; set; }
    }
}
