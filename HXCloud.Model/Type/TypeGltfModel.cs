using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class TypeGltfModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string GltfName { get; set; }
        public string Url { get; set; }
        public int TypeId { get; set; }
        public virtual TypeModel Type { get; set; }
    }
}
