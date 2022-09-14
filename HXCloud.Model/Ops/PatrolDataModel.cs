using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 巡检数据
    /// </summary>
    public class PatrolDataModel : BaseModel, IAggregateRoot
    {
        public string Id { get; set; }
        public DateTime Dt { get; set; }
        public string DeviceSn { get; set; }
        public string DeviceName { get; set; }
        //public string Account { get; set; }
        public string Position { get; set; }
        public string PositionName { get; set; }
        public PatrolImageModel PatrolImage { get; set; }
        public ProductDataModel ProductData { get; set; }
        public WaterAnalysisModel WaterAnalysis { get; set; }
        public DevicePatrolModel DevicePatrol { get; set; }
        public TechniquePatrolModel TechniquePatrol { get; set; }
    }
    public class PatrolImageModel : IAggregateRoot
    {
        public string PatrolId { get; set; }
        /// <summary>
        /// 支持上传多张图片
        /// </summary>
        public string Url { get; set; }
        public PatrolDataModel PatrolData { get; set; }
    }
    /// <summary>
    /// 生产数据
    /// </summary>
    public class ProductDataModel : IAggregateRoot
    {
        public string PatrolId { get; set; }
        public string Content { get; set; }
        public PatrolDataModel PatrolData { get; set; }

    }
    /// <summary>
    /// 水质分析数据
    /// </summary>
    public class WaterAnalysisModel : IAggregateRoot
    {
        public string PatrolId { get; set; }
        public string Content { get; set; }
        public PatrolDataModel PatrolData { get; set; }
    }
    /// <summary>
    /// 设备巡检
    /// </summary>
    public class DevicePatrolModel : IAggregateRoot
    {
        public string PatrolId { get; set; }
        public string Content { get; set; }
        public PatrolDataModel PatrolData { get; set; }
    }
    /// <summary>
    /// 工艺巡检
    /// </summary>
    public class TechniquePatrolModel : IAggregateRoot
    {
        public string PatrolId { get; set; }
        public string Content { get; set; }
        public PatrolDataModel PatrolData { get; set; }
    }

}
