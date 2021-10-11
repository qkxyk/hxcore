using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DataDefineWarnCodeAddDto
    {
        [Required]
        public string DataKey { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
