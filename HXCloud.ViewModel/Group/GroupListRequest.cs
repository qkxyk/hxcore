using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class GroupListRequest
    {
        public string Field { get; set; }
        public string FieldValue { get; set; }

        public string OrderField { get; set; }
        public string OrderType { get; set; }
    }
}
