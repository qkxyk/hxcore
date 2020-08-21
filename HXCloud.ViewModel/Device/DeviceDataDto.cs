using System;
using System.Collections.Generic;
using System.Text;
/* 附带设备的在线信息和设备图片
 * 
 * 
 * 
 * 
 */
namespace HXCloud.ViewModel
{
    public class DeviceDataDto
    {
        public string DeviceSn { get; set; }//设备序列号
        public string DeviceName { get; set; }//设备名称
        public DateTime ProductTime { get; set; } = DateTime.Now;//生产日期
        public DateTime UseTime { get; set; } = DateTime.Now;//投入使用日期
        public string DeviceDescription { get; set; }//设备描述
        #region 设备的完整项目路径和完整项目名称
        public string FullId { get; set; }//项目的完整路径标示，以/分割（主要用于权限验证，不用再递归项目）
        public string FullName { get; set; }//项目完整路径的项目的名称，以/分割
        #endregion

        //设备表中新增验证权限的项目编号,设备权限验证时只需验证此编号即可。
        public string DeviceNo { get; set; }//设备编号
        public string Position { get; set; }//设备的位置坐标
        public string GroupId { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public int TypeId { get; set; }//设备类型
        public string RegionId { get; set; }
        public string Address { get; set; }
        public bool OnLine { get; set; }//设备是否在线
        public List<DeviceImageDto> Images { get; set; }//返回给调用者设备图片
    }
}
