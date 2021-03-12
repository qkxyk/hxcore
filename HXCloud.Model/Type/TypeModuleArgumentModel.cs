using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //模块配置参数，目前主要配置模块的手自动和模块报警复位数据
    public class TypeModuleArgumentModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }//用来标识属于什么，如,T2代表手自动，T5代表报警复位
        public int DataDefineId { get; set; }//类型数据定义标识
        public virtual TypeDataDefineModel TypeDataDefine { get; set; }
        public int ModuleId { get; set; }//模块标示
        public virtual TypeModuleModel TypeModule { get; set; }//关联的类型模块
    }
}
