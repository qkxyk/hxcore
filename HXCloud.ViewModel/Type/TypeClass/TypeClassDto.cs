using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeClassDto
    {
        public int Id { get; set; }
        //分组名称
        public string Name { get; set; }
        //分组的序号，用于显示分组所在的位置
        public int Rank { get; set; }
        public int TypeId { get; set; }//类型编号
    }
}
