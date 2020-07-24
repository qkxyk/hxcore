using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class TypeAccessoryModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ICON { get; set; }//设备配件图标
        public int TypeId { get; set; }
        public TypeModel Type { get; set; }
        public ICollection<TypeAccessoryControlDataModel> TypeAccessoryControlDatas { get; set; }
    }
}
