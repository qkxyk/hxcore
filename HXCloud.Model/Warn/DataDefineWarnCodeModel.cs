using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //关联数据定义和报警编码,不关联数据，需要检测key和code是否存在
    public class DataDefineWarnCodeModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string DataKey { get; set; }//数据定义标识，一个key可能对应多个报警编码，一个报警编码可能对应多个key
        public string Code { get; set; }//报警编码
    }
}
