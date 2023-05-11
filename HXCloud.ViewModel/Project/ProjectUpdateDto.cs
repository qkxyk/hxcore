using HXCloud.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class ProjectUpdateDto
    {
        [Required(ErrorMessage ="项目或者场站标识不能为空")]
        public int Id { get; set; }//项目或者场站编号     
        [Required(ErrorMessage = "项目或者场站名称不能为空")]
        [StringLength(50, ErrorMessage = "项目或者场站名称长度在2到50个字符之间", MinimumLength = 2)]
        public string Name { get; set; }//项目或者场站名称
        //public int ProjectType { get; set; }//项目或者场站
        //public string PathName { get; set; }//完整的项目名称路径
        //public string PathId { get; set; }//完整的项目编号
        public string RegionId { get; set; }//项目区域编号
        //public string AreaId { get; set; }//项目地域编号
        public string Position { get; set; }//项目位置（经纬度）
                                            //public int? ParentId { get; set; }//父项目编号
                                            //public string GroupId { get; set; }//组织编号
        #region 20230324运维增加项目
        public ProjectScale ProjectScale { get; set; }//项目规模
        public string Crafts { get; set; }//生产工艺
        public string WaterIndex { get; set; }//出水指标
        public string DeviceType { get; set; }//设备型号，手工输入场站中设备型号和个数，如：贝斯100T-20台
        public string Address { get; set; }//项目地址或者场站地址
        public string Description { get; set; }//项目描述
        #endregion
    }
}
