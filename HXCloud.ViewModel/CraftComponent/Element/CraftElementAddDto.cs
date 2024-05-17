using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class CraftElementAddDto
    {
        public string Name { get; set; }
        public string Image { get; set; }//图片格式，有可能为空(svg格式，svg内容使用者会放在data中)
        public string Data { get; set; }//组件内容，使用者自己定义内容
        public int? UserId { get; set; }//个人组件用户标识
        public int ElementType { get; set; } = 0;
        //public int CatalogId { get; set; }
    }

    /// <summary>
    /// 工艺组件请求数据
    /// </summary>
    public class CraftElementAddReqest
    {
        [Required(ErrorMessage = "组件名称不能为空")]
        public string Name { get; set; }
        public IFormFile file { get; set; }
        public string Data { get; set; }//组件内容，使用者自己定义内容
        //[Range(0,1,ErrorMessage ="工艺组件类型只输入0和1")]
        //public int ElementType { get; set; } = 0;
    }
}
