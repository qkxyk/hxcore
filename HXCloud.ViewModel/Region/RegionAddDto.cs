using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RegionAddDto
    {
        [Required(ErrorMessage = "区域名称不能为空")]
        [StringLength(50, ErrorMessage = "区域名称长度在2到50个字符之间", MinimumLength = 2)]
        public string Name { get; set; }//区域名称
        public string Point { get; set; }//区域中心点
        public string Radius { get; set; }//区域半径
        public string ParentId { get; set; }//父区域标示
        public string FullPath { get; set; }//区域的完整路径
        public string RegionCode { get; set; } = "101";//区域码，亚太区以1开头，后跟两位数字，中国以01开头，非洲以2开头，北美以3开头，南美以4开头，欧洲以5开头，大洋洲以6开头。这个码用于生成区域编码
    }
}
