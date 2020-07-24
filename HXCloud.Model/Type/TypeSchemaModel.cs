using System;
using System.Collections.Generic;
using System.Linq;

namespace HXCloud.Model
{
    public class TypeSchemaModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }//模式名称
        public string Key { get; set; }//模式对应的key
        public int Value { get; set; }
        public Nullable<int> ParentId { get; set; }
        public virtual ICollection<TypeSchemaModel> Child { get; set; }
        public virtual TypeSchemaModel Parent { get; set; }
        public int TypeId { get; set; }
        public virtual TypeModel Type { get; set; }//关联设备类型，每种设备类型都有相对应的模式
        public SchemaType SchemaType { get; set; } = SchemaType.auto;
        //public List<TypeSchemaModel> GetChild()
        //{
        //    return this.Child.ToList();
        //}
    }
    public enum SchemaType
    {
        auto,
        define
    }
}