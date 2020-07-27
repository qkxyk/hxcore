using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HXCloud.ViewModel
{
    public class ProjectAddDto
    {
        [Required(ErrorMessage = "项目或者场站名称不能为空")]
        public string Name { get; set; }//项目或者场站名称
        [Range(0, 1, ErrorMessage = "只能输入0和1，0表示项目，1表示场站")]
        public int ProjectType { get; set; }//项目或者场站
        public string RegionId { get; set; }//项目区域编号
        public string Position { get; set; }//项目位置（经纬度）
        public int? ParentId { get; set; }//父项目编号
    }
}
