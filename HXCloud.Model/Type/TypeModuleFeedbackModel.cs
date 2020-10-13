using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 模块控制项反馈数据
    /// </summary>
    public class TypeModuleFeedbackModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public int Sn { get; set; } = 0;//处理多个反馈值时序号，可以应对control项的计算公式
        public int ModuleControlId { get; set; }//对应控制项
        public virtual TypeModuleControlModel TypeModuleControl { get; set; }
        public int DataDefineId { get; set; }
        public virtual TypeDataDefineModel TypeDataDefine { get; set; }
    }
}
