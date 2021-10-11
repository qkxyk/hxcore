using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class ProjectPrincipalsAddDto
    {
        [Required(ErrorMessage ="姓名不能为空")]
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
