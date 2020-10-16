using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceHardwareConfigAddDto
    {
        //添加时从类型数据定义获取key、keyname，unit和format
        [Required(ErrorMessage = "类型数据定义标示不能为空")]
        public int DataDefineId { get; set; }
        public int No { get; set; }//分组号
        public int Sn { get; set; }//序号
        //public string Key { get; set; }//类型key,key和keyname和类型数据定义key和name相同
        //public string KeyName { get; set; }//类型key名称
        public string ShowKey { get; set; }//显示类型
        public string KeyType { get; set; }//数据类型
        public string Max { get; set; }//数据最大值（使用者自己转换）
        public string Min { get; set; }//数据最小值
        //public string Unit { get; set; }//数据单位
        //public string Format { get; set; }//数据转换公式
        public string Comm { get; set; }//通讯方式（主要指串口通讯，ModbusTcp和ModbusRTU）
        public int? Port { get; set; }//通讯端口
        public string ModbusSlave { get; set; }         //modbu从站地址
        public string PLCType { get; set; }//plc类型
        public string RegAd { get; set; }   //寄存器地址
        public int CMD { get; set; }//命令
        public int Address { get; set; }//地址
        public int? BitOffSet { get; set; }//偏移量
        public int Lens { get; set; }//数据长度
    }
}
