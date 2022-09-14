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

    /// <summary>
    /// 适用于单个主键
    /// </summary>
    /// <typeparam name="T">主键类型</typeparam>
    public class HandleIdResponse<T> : BaseResponse
    {
        public T Id { get; set; }
    }
}
