using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 获取设备的名称，运维时根据设备序列号获取设备名称
    /// </summary>
    public class DeviceWithNameDto
    {
        public bool IsExist { get; set; }//输入的设备编号是否存在
        public string DeviceNo { get; set; }//设备编号
        public string DeviceSn { get; set; }//设备序列号
        public string DeviceName { get; set; }//设备名称
        public int TypeId { get; set; }
        public string PathId { get; set; }//用来验证用户权限
        public string FullName { get; set; }
    }
}
