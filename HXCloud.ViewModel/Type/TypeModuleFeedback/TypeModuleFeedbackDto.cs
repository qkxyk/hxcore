using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeModuleFeedbackDto
    {
        public int Id { get; set; }
        public int Sn { get; set; } = 0;//处理多个反馈值时序号，可以应对control项的计算公式
        public int ModuleControlId { get; set; }//对应控制项
        public int DataDefineId { get; set; }
        public string Key { get; set; }//关联数据定义的key
        public string Format { get; set; }
        public string Unit { get; set; }
        //20201106更改为showType
        public string ShowType { get; set; }
        public string DefaultValue { get; set; }
    }
}
