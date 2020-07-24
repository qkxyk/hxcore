using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //原来的DeviceConfig和TypeTemplateConfig合并为TypeArgument
    public class TypeArgumentModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }//默认的一些配置数据(约定数据)
        public int DefineId { get; set; } = 0;//
        public virtual TypeDataDefineModel TypeDataDefine { get; set; }
        public int TypeId { get; set; }
        public virtual TypeModel Type { get; set; }
    }
}
