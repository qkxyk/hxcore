using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class WaterAnalysisDto
    {
        public float PHIn { get; set; }
        public float PHOut { get; set; }
        public float CodIn { get; set; }
        public float CodOut { get; set; }
        public float NH3NIn { get; set; }
        public float NH3NOut { get; set; }
        public float TNIn { get; set; }
        public float TNOut { get; set; }
        public float TPIn { get; set; }
        public float TPOut { get; set; }
    }
}
