using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class SiteDto
    {
        public int Id { get; set; }//项目或者场站编号     
        public string Name { get; set; }//项目或者场站名称
        public int ProjectType { get; set; }//项目或者场站
        public string PathName { get; set; }//完整的项目名称路径
        public string PathId { get; set; }//完整的项目编号
        public string RegionId { get; set; }//项目区域编号
        public string RegionName { get; set; }//项目区域名称
        public string Position { get; set; }//项目位置（经纬度）
        public int? ParentId { get; set; }//父项目编号
        public string GroupId { get; set; }//组织编号
        public string Image { get; set; }//获取项目或者场站的图片
        public int DeviceCount { get; set; } //项目下的设备数量
    }
}
