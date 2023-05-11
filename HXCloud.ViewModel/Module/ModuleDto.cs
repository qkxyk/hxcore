using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class ModuleDto
    {
        public int Id { get; set; }//模块标识
        public string ModuleName { get; set; }//模块名称
        public string Code { get; set; }//模块的编号，例如ops代表运维模块，deviceMonitor代表设备监控模块,系统默认是设备监控模块
    }
}
