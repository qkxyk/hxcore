using HXCloud.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class ProjectUpdateInfoDto
    {
        [Required(ErrorMessage = "项目或者场站标识不能为空")]
        public int Id { get; set; }//项目或者场站编号     
        public ProjectScale ProjectScale { get; set; }//项目规模
        public string Crafts { get; set; }//生产工艺
        public string WaterIndex { get; set; }//出水指标
        public string DeviceType { get; set; }//设备型号，手工输入场站中设备型号和个数，如：贝斯100T-20台
        public string Address { get; set; }//项目地址或者场站地址
    }
}
