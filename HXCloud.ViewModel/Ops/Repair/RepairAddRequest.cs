using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairAddRequest
    {
        [Required(ErrorMessage ="必须输入关联的设备编号")]
        public string DeviceSn { get; set; }
        public int? IssueId { get; set; }
        [Range(0,1,ErrorMessage ="只能输入0和1")]
        public int RepairType { get; set; }
        [Range(0,2,ErrorMessage ="只能输入0，1，2")]
        public int EmergenceStatus { get; set; }
        [Required(ErrorMessage ="接单人不能为空")]
        ///// <summary>
        ///// 接单人
        ///// </summary>      
        public string Receiver { get; set; }
        }
}
