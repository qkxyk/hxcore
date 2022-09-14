using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairWaitDto
    {
        [Required(ErrorMessage ="运维单编号不能为空")]
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
