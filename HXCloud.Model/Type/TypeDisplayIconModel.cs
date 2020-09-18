using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 配置类型关联数据定义显示的图标，主要用来设置，类型关联设备的列表显示的图标，以及图标关联的数据定义
    /// </summary>
    public class TypeDisplayIconModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int TypeId { get; set; }
        public int Sn { get; set; } = 0;//排序序号
        public virtual TypeModel Type { get; set; }
        public int DataDefineId { get; set; }
        public virtual TypeDataDefineModel TypeDataDefine { get; set; }
    }
}
