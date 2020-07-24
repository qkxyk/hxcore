using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeAddViewModel
    {
        [Required(ErrorMessage = "类型名称不能为空")]
        [StringLength(100, ErrorMessage = "类型名称长度在2到100个字符之间", MinimumLength = 2)]
        public string TypeName { get; set; }//设备类型名称
        public Nullable<int> ParentId { get; set; } //设备所属的父类型标识
        public string Description { get; set; }//设备类型描述
        //public string PathId { get; set; }
        //public string PathName { get; set; }

        public string ICON { get; set; }//类型图标名称（后台只存图标名称,图标放在客户端）2019-1-10添加
        [Required(ErrorMessage = "类型所属的组织编号不能为空")]
        public string GroupId { get; set; }//组织编号
        //0为目录型，1为数据型，目录型只能添加子节点不能添加其他数据，数据型只能添加类型数据，不能添加子节点
        [Range(0, 1, ErrorMessage = "类型种类只能为0或者1")]
        public int Status { get; set; } = 0;
    }
}
