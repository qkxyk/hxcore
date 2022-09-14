using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class IssueAddRequest
    {
        public string Description { get; set; }
        public List<IFormFile> file { get; set; }
        [Required(ErrorMessage = "设备编号不能为空")]
        public string DeviceSn { get; set; }
    }
}
