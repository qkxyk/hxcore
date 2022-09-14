using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class PatrolDataRequest : BasePageRequest
    {
        public DateTime BeginTime { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
        public DateTime EndTime { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));

    }
}
