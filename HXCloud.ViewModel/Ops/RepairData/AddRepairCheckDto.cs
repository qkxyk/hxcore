using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class AddRepairCheckDto:AddRepairDataBaseDto
    {
        //[Required(ErrorMessage = "运维单编号不能为空")]
        //public string RepairId { get; set; }
        //[Range(0, 5, ErrorMessage = "运维单状态只能是0到5")]
        //public int RepairStatus { get; set; }
        public string Message { get; set; }//审核意见
    }
}
