using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeUpdateFileUpdateViewModel
    {
        [Required(ErrorMessage ="升级文件的标示必须输入")]
        public int Id { get; set; }
        [Required(ErrorMessage ="升级文件的名称必须输入")]
        public string Name { get; set; }
        [Required(ErrorMessage ="升级文件的版本号必须输入")]
        public string Version { get; set; }
        [Required(ErrorMessage ="类型编号必须输入")]
        public int TypeId { get; set; }
        public string Description { get; set; }

    }
}
