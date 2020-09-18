using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeOverviewDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DataDefineKey { get; set; }
        public string Format { get; set; }
        public string Unit { get; set; }
        public string DataType { get; set; }
        public string DefaultValue { get; set; }
        public int Sn { get; set; }  //显示顺序
    }
}
