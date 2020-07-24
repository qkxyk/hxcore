using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class UserStatusUpDateViewModel
    {
        [Required(ErrorMessage ="用户编号不能为空")]
        public int Id { get; set; }
        [Required(ErrorMessage ="状态码不能为空")]
        [Range(0,2,ErrorMessage ="状态码只能为0到2之间的整数")]
        public int Status { get; set; }
    }
}
