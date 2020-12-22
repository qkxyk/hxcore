using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class DeviceMigrationModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        //public string Account { get; set; }
        public string GroupId { get; set; }
        //public DateTime Date { get; set; } = DateTime.Now;
        public int? CurrentPId { get; set; }//当前的场站标示
        public int? PrePId { get; set; }//迁移前的场站标示
        public int TypeId { get; set; }//0为设备添加，1为设备迁移（组织内迁移）
        public string DeviceSn { get; set; }
        public string DeviceNo { get; set; }//跨组织查找
        public DeviceModel Device { get; set; }
    }
}
