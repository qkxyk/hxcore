using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeSchemaData
    {
        public TypeSchemaData()
        {
            Child = new List<TypeSchemaData>();
        }
        public int Id { get; set; }
        public string Name { get; set; }//模式名称
        public string Key { get; set; }//模式对应的key
        public int Value { get; set; }
        public Nullable<int> ParentId { get; set; }
        public int TypeId { get; set; }
        public int SchemaType { get; set; }
        public List<TypeSchemaData> Child { get; set; }//子类型
    }
}
