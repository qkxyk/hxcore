using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public interface ICreate
    {
        public string Create { get; set; }
        public DateTime CreateTime { get; set; }
    }
    public interface IModify
    {
        public string Modify { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
