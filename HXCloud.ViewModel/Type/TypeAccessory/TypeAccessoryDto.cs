﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeAccessoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ICON { get; set; }//设备配件图标
        public int TypeId { get; set; }
        public List<AccessoryControlDto> Controls { get; set; }
    }
    public class AccessoryControlDto
    {
        public int Id { get; set; }
        public string ControlName { get; set; }
        public string DataValue { get; set; }//设备设置值
        public int DataDefineId { get; set; }//对应设备数据栏位编号（和设备栏位对应关系为1：N）

        public int AssociateDefineId { get; set; }//2018-10-15 数据控制新增关联数据栏位字段，满足设备设置值和设备当前值的关联
        public int AccessoryId { get; set; }
        public string SequenceIn { get; set; }//设备组内坐标
        public string SequenceOut { get; set; }//设备组间坐标

        public int IState { get; set; } = 0;//标示此控制是否在android端的控制项中，1表示在，0表示不在

        #region 关联数据定义
        public string DataKey { get; set; }
        public string DataType { get; set; }
        public string Unit { get; set; }
        public string Format { get; set; }
        public bool AutoControl { get; set; }//自动模式下是否可以控制

        #endregion
    }
}
