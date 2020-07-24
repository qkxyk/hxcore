using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    //增删改的返回数据
   public class HandleResponse<T>:BaseResponse
    {
        //增删改影响的数据标识
        public T Key { get; set; }
    }
}
