using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //主要用于导入到设备配置中，简化设备处理
    public class TypeHardwareConfigModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public int No { get; set; }//分组号
        public int Sn { get; set; }//序号
        public string Key { get; set; }//类型key
        public string KeyName { get; set; }//类型key名称
        public string ShowKey { get; set; }//显示类型key
        public string KeyType { get; set; }//数据类型
        public string Max { get; set; }//数据最大值（使用者自己转换）
        public string Min { get; set; }//数据最小值
        public string Unit { get; set; }//数据单位
        public string Format { get; set; }//数据转换公式
        public string Comm { get; set; }//通讯方式（只要指串口通讯，ModbusTcp和ModbusRTU）
        public int? Port { get; set; }//通讯端口
        public string ModbusSlave { get; set; }         //modbu从站地址
        public string PLCType { get; set; }//plc类型
        public string RegAd { get; set; }   //寄存器地址
        public int CMD { get; set; }//命令
        public int Address { get; set; }//地址
        public int? BitOffSet { get; set; }//偏移量
        public int Lens { get; set; }//数据长度
        public int TypeId { get; set; }
        public TypeModel Type { get; set; }
    }
}
