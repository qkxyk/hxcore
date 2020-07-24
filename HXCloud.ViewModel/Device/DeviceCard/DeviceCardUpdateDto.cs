using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceCardUpdateDto
    {
        [Required(ErrorMessage = "流量卡编号必须输入")]
        public string CardNo { get; set; }//卡号
        [Required(ErrorMessage = "流量卡AppId不能为空")]
        public string AppId { get; set; }
        [Required(ErrorMessage = "流量卡AppSecret不能为空")]
        public string AppSecret { get; set; }
        public DateTime? ExpireTime { get; set; }
    }
}
