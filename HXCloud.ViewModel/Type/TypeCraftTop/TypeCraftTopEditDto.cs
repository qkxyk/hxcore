﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeCraftTopEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; }//top名称
        public string Url { get; set; }//top数据对应的url

        public int TypeId { get; set; }
        public int Sn { get; set; } = 0;//数据序号
        public string Key { get; set; }//关键字，同一个类型不能重复
    }
}