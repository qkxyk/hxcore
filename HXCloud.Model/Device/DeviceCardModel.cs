using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class DeviceCardModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string CardNo { get; set; }//卡号
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public DateTime? ExpireTime { get; set; }
        public string DeviceSn { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ICCID { get; set; }
        public string IMEI { get; set; }
        public virtual DeviceModel Device { get; set; }
    }

    public class DeviceCardInfoModel
    {
        public string DeviceSn { get; set; }
        public string DeviceNo { get; set; }
        public string ICCID { get; set; }
        public string DeviceName { get; set; }//设备名称
        public Nullable<int> ProjectId { get; set; }
        public string FullId { get; set; }//项目的完整路径标示，以/分割（主要用于权限验证，不用再递归项目）
        public string FullName { get; set; }//项目完整路径的项目的名称，以/分割
    }
}
