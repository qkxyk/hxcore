using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class BaseCModel : ICreate, IAggregateRoot
    {
        public string Create { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
