using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeImageUpdateViewModel
    {
        [Required(ErrorMessage ="类型图片标示必须输入")]
        public int Id { get; set; }
        [Required(ErrorMessage ="类型图片名称必须输入")]
        public string ImageName { get; set; }
        public int Rank { get; set; } = 1;//图片顺序
        public string Description { get; set; }
        [Required(ErrorMessage = "类型编号必须输入")]
        public int TypeId { get; set; }
    }
}
