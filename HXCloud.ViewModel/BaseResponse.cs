using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class BaseResponse
    {
        //返回操作是否成功
        public bool Success { get; set; }
        //返回操作信息
        public string Message { get; set; }
    }

    public class BResponse<T> : BaseResponse
    {
        public T Data { get; set; }
    }
}
