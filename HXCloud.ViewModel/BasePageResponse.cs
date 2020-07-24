using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class BasePageResponse<T> : BResponse<T>
    {
        public int CurrentPage { get; set; }//当前页
        public int TotalPage { get; set; }                //总页数
        public int Count { get; set; }                                  //记录数
        public int PageSize { get; set; } = 10;
    }
}
