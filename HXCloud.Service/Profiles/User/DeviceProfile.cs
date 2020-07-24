using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            //设备

            //设备流量卡
            CreateMap<DeviceCardAddDto, DeviceCardModel>();
            //设备统计数据

            //设备历史数据

            //设备图片

            //设备视频

            //设备配置

            //设备PLC配置数据

            //设备输入数据

            //设备迁移数据

            //设备在线数据
        }
    }
}
