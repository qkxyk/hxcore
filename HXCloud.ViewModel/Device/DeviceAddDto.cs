using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    //注设备生产日期和投入使用日期一经设定不允许修改
    public class DeviceAddDto
    {
        [Required(ErrorMessage = "设备名称不能为空")]
        [StringLength(128, ErrorMessage = "设备名称长度在3个字符到128个字符之间", MinimumLength = 3)]
        public string DeviceName { get; set; }//设备名称
        public DateTime ProductTime { get; set; } = DateTime.Now;//生产日期
        public DateTime UseTime { get; set; } = DateTime.Now;//投入使用日期
        [StringLength(500, ErrorMessage = "设备描述不能超过500个字符")]
        public string DeviceDescription { get; set; }//设备描述

        //设备编号为设备出厂时设定。
        [Required(ErrorMessage = "设备编号不能为空")]
        public string DeviceNo { get; set; }//设备编号
        public string Position { get; set; }//设备的位置坐标
        //[Required(ErrorMessage = "设备所属组织标示不能空")]
        //public string GroupId { get; set; }
        public Nullable<int> ProjectId { get; set; }
        [Required(ErrorMessage = "设备类型不能为空")]
        public int TypeId { get; set; }//设备类型
        public string RegionId { get; set; }
        public string Address { get; set; }
        [Range(0, 3, ErrorMessage = "水类型只能输入0到3")]
        public int Water { get; set; } = 0;
    }
}
