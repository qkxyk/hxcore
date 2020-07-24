using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class TypeSystemAccessoryModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ICON { get; set; }//设备配件图标

        public int SystemId { get; set; }
        public virtual TypeSystemModel TypeSystem { get; set; }
        public virtual ICollection<TypeSystemAccessoryControlDataModel> TypeSystemAccessoryControlDatas { get; set; }
    }
}
