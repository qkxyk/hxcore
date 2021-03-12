using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeModuleArgumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }//用来标识属于什么，如,T2代表手自动，T5代表报警复位
        public int DataDefineId { get; set; }//类型数据定义标识
        public int ModuleId { get; set; }//模块标示
        public string DataKey { get; set; }//数据标示
        public string DefineFormat { get; set; }

        public string DataName { get; set; }//数据名称
    }
}
