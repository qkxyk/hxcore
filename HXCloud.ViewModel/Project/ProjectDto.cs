using HXCloud.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    //不包含project的子节点
    public class ProjectDto
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
        public int ProjectCount { get; set; }//项目直属项目的数量
        public int SiteCount { get; set; }  //项目下直属的场站数量
        public int DeviceCount { get; set; } //项目下的设备数量

        #region 20230324运维增加项目
        //public ProjectScale ProjectScale { get; set; }//项目规模
        public string Crafts { get; set; }//生产工艺
        public string WaterIndex { get; set; }//出水指标
        public string DeviceType { get; set; }//设备型号，手工输入场站中设备型号和个数，如：贝斯100T-20台
        public string Address { get; set; }//项目地址或者场站地址
        public string ProjectScale { get; set; }
        public string Description { get; set; }//项目描述
        //public ProjectDto()
        //{
        //    ProjectScales = ProjectScale?.ToString();
        //}
        #endregion
    }

    //不包含project的子节点
    public class ProjectsDto
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
        public int ProjectCount { get; set; }//项目直属项目的数量
        public int SiteCount { get; set; }  //项目下直属的场站数量
    }
}
