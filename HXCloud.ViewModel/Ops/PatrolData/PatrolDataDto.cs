using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class PatrolDataDto
    {
        public string Id { get; set; }
        public DateTime Dt { get; set; }
        public string DeviceSn { get; set; }
        public string DeviceName { get; set; }
        //public string Account { get; set; }
        public string Position { get; set; }
        public string PositionName { get; set; }
        public string PatrolImage { get; set; }
        public string ProductData { get; set; }
        public string WaterAnalysis { get; set; }
        public string DevicePatrol { get; set; }
        public string TechniquePatrol { get; set; }
    }
}
