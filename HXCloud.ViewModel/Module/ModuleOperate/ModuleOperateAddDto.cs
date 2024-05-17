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

        //public int ModuleId { get; set; }//所属模块标识
        [Required(ErrorMessage = "操作码不能为空")]
        public string Code { get; set; }//操作码，用于权限验证，命名规范，采用大驼峰命名法
        [Required(ErrorMessage ="编号不能为空")]
        public int SerialNumber { get; set; }//编号,用来给操作进行分组，同一个分组内Code不能重复
        [Required(ErrorMessage ="编号名称不能为空")]
        public string SerialName { get; set; }//编号名称
    }

    public class ModuleOperateDto
    {
        public int Id { get; set; }//操作标识
        public string OperateName { get; set; }//操作名称
        public int ModuleId { get; set; }//所属模块标识
        public string Code { get; set; }//操作码，用于权限验证
        public int SerialNumber { get; set; }//编号,用来给操作进行分组，同一个分组内Code不能重复
        public string SerialName { get; set; }//编号名称
    }
}
