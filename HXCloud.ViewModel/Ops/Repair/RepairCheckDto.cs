using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 核验维修或者调试单是否合格
    /// </summary>
    public class RepairCheckDto
    {
        public string Id { get; set; }
        public bool Check { get; set; } = true;
        public string Description { get; set; }
        public string CheckAccount { get; set; }//核验人
        public string CheckAccountName { get; set; }//冗余审核人员
    }
}
