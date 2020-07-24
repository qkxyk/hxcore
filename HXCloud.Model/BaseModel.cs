using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class BaseModel : BaseCModel, IModify
    {
        public string Modify { get; set; }//修改人
        public Nullable<DateTime> ModifyTime { get; set; }//修改时间
    }
}
