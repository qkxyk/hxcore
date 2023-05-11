using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class OpsStatisticsResponseDto
    {
        //public DebugStatisticsDto Debug { get; set; }
        public RepairStatisticsDto Debug { get; set; }
        public RepairStatisticsDto Repair { get; set; }
    }
}
