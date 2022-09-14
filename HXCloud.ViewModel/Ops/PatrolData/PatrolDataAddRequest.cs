using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class PatrolDataAddRequest
    {
        [Required(ErrorMessage = "关联的设备编号不能为空")]
        public string DeviceSn { get; set; }
        //[Required]
        public string Position { get; set; }
        public string PositionName { get; set; }
    }
}
