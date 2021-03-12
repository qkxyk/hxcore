using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 类型模块
    /// </summary>
    public class TypeModuleModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public ModuleType ModuleType { get; set; }
        public int Sn { get; set; } = 0;//模块的先后顺序，主要用来排序从模块的先后顺序
        public int TypeId { get; set; }
        public virtual TypeModel Type { get; set; }
        public virtual ICollection<TypeModuleControlModel> ModuleControls { get; set; }
        public virtual ICollection<TypeModuleArgumentModel> ModeleArguments { get; set; }

    }
    public enum ModuleType
    {
        Master = 1, Slave
    }
}
