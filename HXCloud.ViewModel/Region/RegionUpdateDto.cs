using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RegionUpdateDto
    {
        [Required(ErrorMessage ="区域标示不能为空")]
        public string Id { get; set; }//区域标示
        [Required(ErrorMessage ="区域名称不能为空")]
        [StringLength(50,ErrorMessage ="区域名称长度在2到50个字符之间",MinimumLength =2)]
        public string Name { get; set; }//区域名称
        public string Point { get; set; }//区域中心点
        public string Radius { get; set; }//区域半径
        public string ParentId { get; set; }//父区域标示
        public string FullPath { get; set; }//区域的完整路径
        public string GroupId { get; set; }
    }
}
