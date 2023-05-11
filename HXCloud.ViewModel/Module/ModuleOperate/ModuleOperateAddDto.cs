using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class ModuleOperateAddDto
    {
        [Required(ErrorMessage ="操作名称不能为空")]
        public string OperateName { get; set; }//操作名称
        [Required(ErrorMessage ="模块编号不能为空")]
        public int ModuleId { get; set; }//所属模块标识
    }

    public class ModuleOperateDto
    {
        public int Id { get; set; }//操作标识
        public string OperateName { get; set; }//操作名称
        public int ModuleId { get; set; }//所属模块标识
    }
}
