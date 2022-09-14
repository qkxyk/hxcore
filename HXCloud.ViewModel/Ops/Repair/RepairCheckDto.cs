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
        [Required(ErrorMessage = "运维单号不能为空")]
        public string Id { get; set; }
        public bool Check { get; set; } = true;
        public string Description { get; set; }
    }
}
