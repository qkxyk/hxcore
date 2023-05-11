using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class PatrolDataAddDto
    {
        public string DeviceSn { get; set; }
        public string DeviceName { get; set; }
        //[Required]
        public string Position { get; set; }
        public string PositionName { get; set; }
        public string ProjectName { get; set; }
        public string CreateName { get; set; }
    }
}
