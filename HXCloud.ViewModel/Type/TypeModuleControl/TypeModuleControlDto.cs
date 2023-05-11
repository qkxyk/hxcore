using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeModuleControlDto
    {
        public int Id { get; set; }
        public string ControlName { get; set; }
        public string DataValue { get; set; }//设备设置值
        public string Formula { get; set; }   //公式，用于反馈项的设置
        public int Sn { get; set; } = 0;//显示顺序
        public int DataDefineId { get; set; }//对应设备数据栏位编号（和设备栏位对应关系为1：N）
        public string Key { get; set; }
        public int ModuleId { get; set; }//模块标示
        public string Format { get; set; }
        public string Unit { get; set; }
        //20201106更改为showType
        public string ShowType { get; set; }
        public string DefaultValue { get; set; }
        public bool AutoControl { get; set; } = false;//自动模式下是否可以控制
                                                      //20201230新增分组信息
        public int ClassId { get; set; }//分组标识
        public string ClassName { get; set; }//分组名称
        public int Rank { get; set; }//分组序号
        public TypeModuleControlDto()
        {
            FeedBacks = new List<TypeModuleFeedbackDto>();
        }
        public List<TypeModuleFeedbackDto> FeedBacks { get; set; }


    }

    public class TypeModuleControlMockDto
    {
        public int Id { get; set; }
        public string ControlName { get; set; }
        public string Formula { get; set; }   //公式，用于反馈项的设置
        public string Format { get; set; }
        public string Unit { get; set; }
        //20201106更改为showType
        public string ShowType { get; set; }
        public string ClassName { get; set; }//分组名称
    }
}
