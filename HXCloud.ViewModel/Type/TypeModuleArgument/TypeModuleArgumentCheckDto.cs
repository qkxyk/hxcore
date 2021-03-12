using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeModuleArgumentCheckDto
    {
        public bool IsExist { get; set; }//输入的设备编号是否存在
        public string GroupId { get; set; }//获取模块所属的组织
        public int TypeId { get; set; }//类型标识
    }
}
