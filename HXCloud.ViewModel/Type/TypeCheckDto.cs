using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeCheckDto
    {
        public int Status { get; set; }//类型状态
        public bool IsExist { get; set; }//类型是否存在
        public string GroupId { get; set; }
    }
}
