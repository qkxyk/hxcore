using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 上传工艺组件类型图标，需要转换dto
    /// </summary>
    public class CraftComponentCatalogAddRequest
    {
        [Required(ErrorMessage = "分类名称不能为空")]
        [StringLength(25, ErrorMessage = "分类名称长度在2到25个字符之间", MinimumLength = 2)]
        public string Name { get; set; }
        [Required(ErrorMessage = "图片不能为空")]
        public IFormFile file { get; set; }
        public int? ParentId { get; set; }
        //public int MyProperty { get; set; }
        [Range(0,1,ErrorMessage ="工艺组件类型只能是公开或者私有")]
        public int CraftType { get; set; }//工艺组件是个人或者公共的
    }
}
