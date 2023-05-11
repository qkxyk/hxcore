using HXCloud.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class ProjectModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }//项目或者场站编号     
        public string Name { get; set; }//项目或者场站名称
        public ProjectType ProjectType { get; set; }//项目或者场站
        public string PathName { get; set; }//完整的项目名称路径
        public string PathId { get; set; }//完整的项目编号
        public string RegionId { get; set; }//项目区域编号
        //public string AreaId { get; set; }//项目地域编号
        public string Position { get; set; }//项目位置（经纬度）
        public int? ParentId { get; set; }//父项目编号
        public virtual ProjectModel Parent { get; set; } //父项目信息
        public virtual ICollection<ProjectModel> Child { get; set; }//子项目集合
        public string GroupId { get; set; }//组织编号
        public virtual GroupModel Group { get; set; }//组织信息
        public virtual ICollection<RoleProjectModel> RoleProjects { get; set; }//项目角色信息
        public ICollection<ProjectImageModel> Images { get; set; }//项目或者场站图片
        public virtual ICollection<DeviceModel> Devices { get; set; }

        public ICollection<ProjectPrincipalsModel> Principals { get; set; }//项目运维人员信息
        #region 20230324运维增加项目
        public ProjectScale ProjectScale { get; set; }//项目规模
        public string Crafts { get; set; }//生产工艺
        public string WaterIndex { get; set; }//出水指标
        public string DeviceType { get; set; }//设备型号，手工输入场站中设备型号和个数，如：贝斯100T-20台
        public string Address { get; set; }//项目地址或者场站地址
        public string Description { get; set; }//项目描述
        #endregion
    }
  
    //项目类型，0为项目，1为项目下的场站
    public enum ProjectType
    {
        Project = 0, Site
    }
}
