﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DataDefineLibraryPageRequest : BasePageRequest
    {
        ////过滤字段和过滤值
        //[Required(ErrorMessage = "类型标示不能为空")]
        //public int TypeId { get; set; }
        public int CategoryId { get; set; } = 0;
    }
}