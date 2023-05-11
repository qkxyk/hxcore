﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairAddDto
    {
        public string DeviceSn { get; set; }
        public string DeviceName { get; set; }
        public string ProjectName { get; set; }
        public int IssueId { get; set; } = 0;//默认为0，表示未关联问题单
        public int RepairType { get; set; }
        public int EmergenceStatus { get; set; }
        //public string Url { get; set; }
        ///// <summary>
        ///// 接单人
        ///// </summary>      
        public string Receiver { get; set; }
        /// <summary>
        /// 接单人电话
        /// </summary>
        public string ReceivePhone { get; set; }
        //派单时问题的描述
        public string Description { get; set; }
        public string CreateName { get; set; }//冗余创建者
        public string ReceiverName { get; set; }//冗余接单人
    }
}
