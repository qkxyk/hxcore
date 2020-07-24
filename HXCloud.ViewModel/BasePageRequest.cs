using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HXCloud.ViewModel
{
    public class BasePageRequest : BaseRequest
    {
        [Range(1, 10000, ErrorMessage = "超出范围")]
        public int PageNo { get; set; } = 1;//当前页
        public int PageSize { get; set; } = 10;//页大小
       
    }
}
