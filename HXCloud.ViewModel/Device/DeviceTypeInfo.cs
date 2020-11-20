using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceTypeInfo
    {
        public string TypeName { get; set; }//设备类型名称
        public int TypeId { get; set; }//设备类型标示
        public string Icon { get; set; }//类型图标
        public int Num { get; set; }//该设备类型关联的设备数量
        public int OnlineNum { get; set; }   //在线设备数量
    }
    public class RegionTypeInfo
    {
        public string RegionName { get; set; }
        public string RegionId { get; set; }
        public int Num { get; set; }
    }
    public class DeviceOverView
    {
        #region 设备信息
        public string DeviceSn { get; set; }//设备序列号
        public string DeviceNo { get; set; }//设备编号
        public string DeviceName { get; set; }//设备名称
        public string GroupId { get; set; }
        public int TypeId { get; set; }//设备类型
        public string Online { get; set; }

        #endregion
    }
}
