using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HXCloud.ViewModel
{
    public class UserPictureViewModel
    {
        public int Id { get; set; }
        public string Test { get; set; }
        public IFormFile file { get; set; }
    }
}
