using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 类型模块控制项
    /// </summary>
    public class TypeModuleControlModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string ControlName { get; set; }
        public string DataValue { get; set; }//设备设置值
        public string Formula { get; set; }   //公式，用于反馈项的设置
        public int Sn { get; set; } = 0;//显示顺序
        public int DataDefineId { get; set; }//对应设备数据栏位编号（和设备栏位对应关系为1：N）
        public virtual TypeDataDefineModel TypeDataDefine { get; set; }
        public int ModuleId { get; set; }//模块标示
        public virtual TypeModuleModel TypeModule { get; set; }//关联的类型模块
        public virtual ICollection<TypeModuleFeedbackModel> TypeModuleFeedbacks { get; set; }//控制项的反馈数据

        public int? ClassId { get; set; }//分组编号
        public virtual TypeClassModel TypeClass { get; set; }//分组信息
    }
}
