using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class CraftComponentCatalogAddDto
    {
        [Required(ErrorMessage = "组件类型名称不能为空")]
        public string Name { get; set; }
        public string Icon { get; set; }//组件类型图标
        public int? ParentId { get; set; }
        public int CraftType { get; set; }//工艺组件是个人或者公共的
    }
}
