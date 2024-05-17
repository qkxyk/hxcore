using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeCraftTopAddRequest
    {
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }//top名称
        [Required(ErrorMessage = "数据内容不能为空")]
        public string Data { get; set; }//top数据对应的url

        public int Sn { get; set; } = 0;//数据序号
        [Required(ErrorMessage ="关键字不能为空")]
        public string Key { get; set; }//关键字，同一个类型不能重复
    }
}
