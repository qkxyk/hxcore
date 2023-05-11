using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class AddRepairDataBaseDto
    {
        [Required(ErrorMessage = "运维单编号不能为空")]
        public string RepairId { get; set; }//关联工单编号
    }
}
