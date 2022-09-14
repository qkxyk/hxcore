using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class BoxBatchAddDto
    {
        public string UMark { get; set; } = "U";
        public string DType { get; set; } = "IG001";
        public string Version { get; set; } = "1";
        public string Year { get; set; }//盒子生产年份，最终只保留后两位
        public int Week { get; set; }//第几周
        public string Station { get; set; } = "C";//工位
        public int BeginNum { get; set; }//起始号
        public int Num { get; set; }//录入的个数
    }
}
