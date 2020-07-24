using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeConfigData
    {
        public int Id { get; set; }
        public string DataName { get; set; }
        public string DataType { get; set; }//配置类型，使用者定义
        public string DataValue { get; set; }
        public int TypeId { get; set; }
    }
}
