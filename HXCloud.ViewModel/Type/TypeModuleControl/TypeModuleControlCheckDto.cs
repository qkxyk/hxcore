using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeModuleControlCheckDto
    {
        public int ModuleId { get; set; }

        public bool IsExist { get; set; }//输入的设备编号是否存在
    }
}
