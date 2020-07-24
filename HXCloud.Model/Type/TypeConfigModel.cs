using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //配置类型的数据,对应原来的TypeTemplateArgs,设备上的配置数据可以有两种方案生成，1、直接从类型导入，2设备添加和类型上的配置合并。注：目前倾向于第一种
    public class TypeConfigModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string DataName { get; set; }
        public string DataType { get; set; }//配置类型，使用者定义
        public string DataValue { get; set; }
        public int TypeId { get; set; }
        public virtual TypeModel Type { get; set; }
    }
}
