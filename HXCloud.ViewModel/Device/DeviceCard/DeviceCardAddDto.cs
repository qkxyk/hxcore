using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 设备流量卡数据，所有字段都可以为空
    /// </summary>
    public class DeviceCardAddDto
    {
        // [Required(ErrorMessage = "流量卡编号必须输入")]
     
        public string CardNo { get; set; }//卡号
                                          // [Required(ErrorMessage = "流量卡AppId不能为空")]
        public string AppId { get; set; }
        //  [Required(ErrorMessage = "流量卡AppSecret不能为空")]
        public string AppSecret { get; set; }
        public DateTime? ExpireTime { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ICCID { get; set; }
        public string IMEI { get; set; }
    }
}
