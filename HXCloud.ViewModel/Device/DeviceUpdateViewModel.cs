using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceUpdateViewModel
    {
        [Required(ErrorMessage ="设备序列号不能为空")]
        public string DeviceSn { get; set; }//设备序列号
        [Required(ErrorMessage = "设备名称不能为空")]
        [StringLength(128, ErrorMessage = "设备名称长度在3个字符到128个字符之间", MinimumLength = 3)]
        public string DeviceName { get; set; }//设备名称
        [StringLength(500, ErrorMessage = "设备描述不能超过500个字符")]
        public string DeviceDescription { get; set; }//设备描述
        public string Position { get; set; }//设备的位置坐标
        //public int TypeId { get; set; }//设备类型
        public string RegionId { get; set; }
        public string Address { get; set; }
        [Range(0, 3, ErrorMessage = "水类型只能输入0到3")]
        public int Water { get; set; } = 0;
    }
}
