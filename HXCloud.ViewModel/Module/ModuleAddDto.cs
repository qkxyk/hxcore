using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class ModuleAddDto
    {
        [Required(ErrorMessage ="模块名称不能为空")]
        public string ModuleName { get; set; }//模块名称
        [Required(ErrorMessage ="模块编号不能为空")]
        public string Code { get; set; }//模块的编号，例如ops代表运维模块，deviceMonitor代表设备监控模块,系统默认是设备监控模块
    }
}
