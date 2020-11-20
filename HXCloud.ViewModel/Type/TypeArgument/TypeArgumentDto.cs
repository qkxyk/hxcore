using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeArgumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }//默认的一些配置数据(约定数据)
        public int DefineId { get; set; } = 0;//数据定义标示
        public int TypeId { get; set; }
        public string DefineKey { get; set; }
        public string DefineType { get; set; }
        public string DefineUnit { get; set; }
        public string DefineFormat { get; set; }

    }
}
