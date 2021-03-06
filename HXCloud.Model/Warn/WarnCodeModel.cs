﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class WarnCodeModel : BaseModel, IAggregateRoot
    {
        public string Description { get; set; }//报警描述
        public string Code { get; set; }//报警编码,报警、故障或者通知编码，唯一
        public int WarnTypeId { get; set; }
        public virtual WarnTypeModel WarnType { get; set; }
        public virtual ICollection<WarnModel> Warn { get; set; }
    }
}
