using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairPartAddDto
    {
        [Required(ErrorMessage = "关联的工单编号不能为空")]
        public string RepairId { get; set; }
        [Range(1, 10, ErrorMessage = "配件数量只能输入1到10")]
        public int Num { get; set; } = 1;//配件数量
        [Required(ErrorMessage ="配件信息不能为空")]
        public string Data { get; set; }//配件数据，因为无法确定配件规格，暂用Data记录运维的配件数据
    }
}
