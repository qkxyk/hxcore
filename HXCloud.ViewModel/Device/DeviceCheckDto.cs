using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 用户设备的组织和设备所属的项目，主要用来验证用户是否对该设备是否有权限
    /// </summary>
    public class DeviceCheckDto
    {
        public string GroupId { get; set; }
        public int? ProjectId { get; set; }
        public string PathId { get; set; }//用来验证用户权限
        public bool IsExist { get; set; }//输入的设备编号是否存在
        public int TypeId { get; set; }
    }
}
