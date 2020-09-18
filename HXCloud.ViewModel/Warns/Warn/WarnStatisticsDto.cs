using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class WarnStatisticsDto
    {
        public int? ProjectId { get; set; }
        public string DeviceSn { get; set; }
        public bool IsDevice { get; set; } = false;
    }
}
