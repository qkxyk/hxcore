using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeData
    {
        public TypeData()
        {
            Child = new List<TypeData>();
        }
        public int Id { get; set; }//设备类型标识
        public string TypeName { get; set; }//设备类型名称
        public Nullable<int> ParentId { get; set; } //设备所属的父类型标识
        public string Description { get; set; }//设备类型描述
        public string PathId { get; set; }
        public string PathName { get; set; }
        public string ICON { get; set; }//类型图标名称（后台只存图标名称,图标放在客户端）2019-1-10添加
        public string GroupId { get; set; }//组织编号
        public int Status { get; set; }

        public List<TypeData> Child { get; set; }//子类型
    }
}
