using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceVideoDto
    {
        public int Id { get; set; }
        public string VideoName { get; set; }//摄像头名称
        public string Url { get; set; }//web视频播放地址
        public string VideoSn { get; set; }//视频设备的序列号
        public int Channel { get; set; } = 1;//设备的通道编号，IPC（摄像机）固定为1，NVR可能有多个通道
        public string SecurityCode { get; set; }//视频加密密码，即设备标签上的6位字母验证码，支持明文/密文两种格式,注意只有开启视频加密的时候需要填入
        public string DeviceSn { get; set; }//设备序列号
        public string Appkey { get; set; }
        public string Secret { get; set; }
        public string AccessToken { get; set; }
        public long? ExpireTime { get; set; }//过期时间
        public string ApiUrl { get; set; }//视频获取token的地址
    }
    #region 萤石数据定义
    public class YSReturnMessage : BaseResponse
    {
        public string Code { get; set; }
        public string Msg { get; set; }
    }
    public class YSData
    {
        public long ExpireTime { get; set; }
        public string AccessToken { get; set; }
    }
    public class YSReturnData : YSReturnMessage
    {
        public YSData Data { get; set; }
        public YSReturnData()
        {
            // Data = new YSData();
        }
    }
    #endregion
}
