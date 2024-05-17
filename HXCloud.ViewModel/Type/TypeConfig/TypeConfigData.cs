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
        public int Category { get; set; }//设备默认为1，类型的数据默认为0
        public string Position { get; set; }//配置数据的坐标
    }
}
