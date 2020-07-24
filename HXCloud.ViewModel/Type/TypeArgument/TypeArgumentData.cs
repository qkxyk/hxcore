using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeArgumentData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }//默认的一些配置数据(约定数据)
        public int DefineId { get; set; } = 0;//
        public int TypeId { get; set; }
    }
}
