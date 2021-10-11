using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DataDefineWarnCodeRequest
    {
        public string DataKeys { get; set; }
        public string Codes { get; set; }

        public bool Flag { get; set; } = false;//false是根据codes查找，true是根据keys查找
    }
}
