using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 配置类型关联的数据定义，用来处理同一类的设备显示的概览数据，关联的数据定义用来解析实时数据
    /// </summary>
    public class TypeOverviewModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }//显示的名称
        public int Sn { get; set; } = 0;//排序序号
        public virtual TypeModel Type { get; set; }
        public int TypeDataDefineId { get; set; }
        public virtual TypeDataDefineModel TypeDataDefine { get; set; }
    }
}
