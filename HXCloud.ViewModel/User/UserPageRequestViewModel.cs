using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class UserPageRequestViewModel : BasePageRequest
    {
        //过滤字段和过滤值
        public int? Status { get; set; }
    }
}
