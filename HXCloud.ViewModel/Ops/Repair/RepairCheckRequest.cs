using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairCheckRequest
    {
        [Required(ErrorMessage = "运维单号不能为空")]
        public string Id { get; set; }
        public bool Check { get; set; } = true;
        public string Description { get; set; }
    }
}
