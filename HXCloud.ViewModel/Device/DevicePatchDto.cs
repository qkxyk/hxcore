using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DevicePatchDto
    {
        [Range(0,3,ErrorMessage = "水类型只能输入0到3")]
        public int Water { get; set; } 
        [Required(ErrorMessage ="设备名称不能为空")]
        public string  DeviceName { get; set; }
        public string DeviceDescription { get; set; }//设备描述
        public string Position { get; set; }//设备的位置坐标
        public string Address { get; set; }
    }
}
