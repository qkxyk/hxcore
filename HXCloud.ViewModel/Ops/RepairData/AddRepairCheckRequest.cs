using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class AddRepairCheckRequest
    {
        [Required(ErrorMessage = "运维单编号不能为空")]
        public string RepairId { get; set; }
        public bool Check { get; set; } = true;//审核是否通过
        public string Message { get; set; }//审核意见
    }
}
