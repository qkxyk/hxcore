using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class PlcSecurityAddDto
    {
        [Required(ErrorMessage = "Key值不能为空")]
        [StringLength(6, ErrorMessage = "key值长度为6", MinimumLength = 6)]
        public string SecurityKey { get; set; }
        public string GPS { get; set; }
    }
}
