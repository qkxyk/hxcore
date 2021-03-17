using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class PlcSecurityModel: BaseCModel
    {
        public int Id { get; set; }
        public string SecurityKey { get; set; }
        public string GPS { get; set; }
    }
}
