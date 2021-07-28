using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HXCloud.ViewModel
{
    public class TypeGltfAddDto
    {
        public IFormFile file { get; set; }
    }
}
