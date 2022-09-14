using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class BoxAddDto
    {
        [Required(ErrorMessage ="UUID不能为空")]
        public string UUId { get; set; }//盒子标识
        //public bool Activate { get; set; } = false;//盒子是否激活
        //public int Num { get; set; } = 0;//盒子激活次数
    }
}
