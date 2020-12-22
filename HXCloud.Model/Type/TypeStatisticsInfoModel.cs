using System.Collections.Generic;

namespace HXCloud.Model
{
    public class TypeStatisticsInfoModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string DataKey { get; set; }//要统计的数据,
        public StatisticsType StaticsType { get; set; }//要统计的是瞬时值或是累计值,1为瞬时值，2为累计值
        public string Standard { get; set; }//标准值
        public string SUnit { get; set; }//标准值单位
        public string Name { get; set; }//要统计的数据的名称
        public int DisplayType { get; set; }//初步定义0为检测值，1为监测，2为状态
        public int ShowState { get; set; } //0表示设备级，1代表场站级，2代表项目级，3代表首页级，他们是包含级别，1包含0和1
        public string Filter { get; set; }//过滤的内容
        public int FilterType { get; set; } //过滤方案，0为不过滤，1为最大值最小值过滤，2为其他方案
        public string Description { get; set; }
        public int TypeId { get; set; }//类型表示
        public virtual TypeModel Type { get; set; }



    }
    public enum StatisticsType
    {
        Instant = 1,//瞬时值
        Total, //累计值
        Discrete //离散型数据(离散型数据需要按时间点把所有数据都累积起来[dt,{data}])
    }
}