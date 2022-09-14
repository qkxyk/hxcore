using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class PatrolItemAddDto
    {
        [Required(ErrorMessage = "巡检单编号不能为空")]
        public string PatrolId { get; set; }
        public string Content { get; set; }
        [Required]
        [Range(0,3,ErrorMessage = "巡检类型必须在 {1} 和 {2}之间.")]
        public int Type { get; set; }
    }
}
