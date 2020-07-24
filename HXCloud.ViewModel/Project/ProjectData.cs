using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class ProjectData
    {
        public int Id { get; set; }//项目或者场站编号     
        public string Name { get; set; }//项目或者场站名称
        public int ProjectType { get; set; }//项目或者场站
        public string PathName { get; set; }//完整的项目名称路径
        public string PathId { get; set; }//完整的项目编号
        public string RegionId { get; set; }//项目区域编号
        //public string AreaId { get; set; }//项目地域编号
        public string Position { get; set; }//项目位置（经纬度）
        public int? ParentId { get; set; }//父项目编号
        public List<ProjectData> Child { get; set; }//子项目集合
        public string GroupId { get; set; }//组织编号
        public ProjectData()
        {
            Child = new List<ProjectData>();
        }
    }
}
