using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class DeviceModel : BaseModel, IAggregateRoot
    {
        public string DeviceSn { get; set; }//设备序列号
        public string DeviceName { get; set; }//设备名称
        public DateTime ProductTime { get; set; } = DateTime.Now;//生产日期
        public DateTime UseTime { get; set; } = DateTime.Now;//投入使用日期
        public string DeviceDescription { get; set; }//设备描述
        #region 设备的完整项目路径和完整项目名称
        public string FullId { get; set; }//项目的完整路径标示，以/分割（主要用于权限验证，不用再递归项目）
        public string FullName { get; set; }//项目完整路径的项目的名称，以/分割
        #endregion

        //2018-4-16新增设备编号，原来的设备序列号改为系统自动生成，guid转换为字符串,设备编号为手动输入和设备关联，系统中可以存在不同组织相同设备编号的设备
        //设备表中新增验证权限的项目编号,设备权限验证时只需验证此编号即可。
        public string DeviceNo { get; set; }//设备编号
        public string Position { get; set; }//设备的位置坐标

        //public DateTime ScrapTime { get; set; }= DateTime.Now;//报废日期
        //public int Primary { get; set; }//设备是否主站，1为主站，其他为从站

        public string GroupId { get; set; }
        public virtual GroupModel Group { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public virtual ProjectModel Project { get; set; }
        public int TypeId { get; set; }//设备类型
        public virtual TypeModel Type { get; set; }
        public virtual DeviceOnlineModel DeviceOnline { get; set; }
        public virtual ICollection<DeviceHisDataModel> DeviceHisData { get; set; }
        //public virtual ICollection<DeviceAlarmModel> DeviceAlarm { get; set; }
        //public virtual ICollection<WarnModel> Warn { get; set; }
        public virtual ICollection<DeviceImageModel> DeviceImage { get; set; }
        public virtual ICollection<DeviceVideoModel> DeviceVideo { get; set; }

        //区域标示，区域和设备之间是松散的关系,新版中去除地域（设备通过定位获取)
        public string RegionId { get; set; }
        public string Address { get; set; }

        //2018-10-23新增巡检和维修单（此功能为临时功能，后期需制定相应的规则）
        //public virtual ICollection<RepairModel> Repairs { get; set; }
        //public virtual ICollection<PollModel> Polls { get; set; }

        public virtual ICollection<DeviceConfigModel> DeviceConfig { get; set; }

        #region 2018-12-7新增巡检周期,每个设备都可以选巡检周期
        //public Nullable<int> CycleId { get; set; }
        //public virtual PollCycleModel PollCycle { get; set; }
        //public Nullable<DateTime> PollDate { get; set; }//巡检日期
        //public Nullable<DateTime> NextPollDate { get; set; }//下次巡检日期，生成巡检报告时自动更新巡检日期和下次巡检日期
        //public virtual ICollection<DevicePollModel> DevicePoll { get; set; }
        #endregion

        #region 2019-1-7新增设备的统计数据(每个设备每一小时取一个值保存下来)
        public virtual ICollection<DeviceStatisticsDataModel> StatisticsData { get; set; }
        #endregion
        public DeviceCardModel DeviceCard { get; set; }
        public ICollection<DeviceHardwareConfigModel> DeviceHardwareConfig { get; set; }
        public ICollection<DeviceInputDataModel> DeviceInputData { get; set; }
        public ICollection<DeviceMigrationModel> DeviceMigration { get; set; }
        public ICollection<DeviceLogModel> DeviceLog { get; set; }
        public ICollection<WarnModel> Warns { get; set; }

    }
}
