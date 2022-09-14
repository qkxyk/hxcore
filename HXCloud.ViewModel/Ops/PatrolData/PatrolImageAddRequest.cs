using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class PatrolImageAddRequest
    {
        [Required(ErrorMessage = "巡检单编号不能为空")]
        public string PatrolId { get; set; }
        public List<IFormFile> file { get; set; }
    }
}
