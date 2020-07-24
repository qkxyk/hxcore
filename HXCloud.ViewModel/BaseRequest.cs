using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class BaseRequest
    {
        public string Search { get; set; }//搜索       

        public string OrderBy { get; set; }
        public string OrderType { get; set; }
    }
}
