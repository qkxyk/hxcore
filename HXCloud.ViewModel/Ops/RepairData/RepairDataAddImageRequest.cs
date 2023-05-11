using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairDataAddImageRequest : AddRepairDataBaseDto
    {
        public List<IFormFile> file { get; set; }
        public string Message { get; set; }
    }
}
