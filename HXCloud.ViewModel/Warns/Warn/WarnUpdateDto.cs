using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class WarnUpdateDto
    {
        [Required]
        public int Id { get; set; }
        public string Comments { get; set; }//报警处理意见
        public string Handler { get; set; }//报警处理人和modify可能不是同一个人
    }
}
