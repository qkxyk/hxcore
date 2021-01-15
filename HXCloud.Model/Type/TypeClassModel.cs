using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 分组，给类型模块下的控制项进行分组
    /// </summary>
    public class TypeClassModel : BaseModel
    {
        //分组编号,主键
        public int Id { get; set; }
        //分组名称
        public string Name { get; set; }
        //分组的序号，用于显示分组所在的位置
        public int Rank { get; set; }
        public int TypeId { get; set; }//类型编号
        public virtual TypeModel Type { get; set; }
        public virtual ICollection<TypeModuleControlModel> TypeModuleControls { set; get; }//关联的模块控制项
    }
}
