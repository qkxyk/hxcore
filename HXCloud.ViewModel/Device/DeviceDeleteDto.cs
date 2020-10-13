using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceDeleteDto
    {
        [Required(ErrorMessage = "设备序列号不能为空")]
        public string DeviceSn { get; set; }
    }
}
