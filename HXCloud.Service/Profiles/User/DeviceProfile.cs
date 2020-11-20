﻿using System;
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
            CreateMap<DeviceAddDto, DeviceModel>();
            CreateMap<DeviceUpdateViewModel, DeviceModel>();
            CreateMap<DeviceModel, DeviceDataDto>().ForMember(dest => dest.OnLine, opt => opt.MapFrom(src => src.DeviceOnline == null ? false : src.DeviceOnline.State))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.DeviceImage));

            //设备流量卡
            CreateMap<DeviceCardAddDto, DeviceCardModel>();
            CreateMap<DeviceCardUpdateDto, DeviceCardModel>();
            CreateMap<DeviceCardModel, DeviceCardDto>();
            //设备操作日志
            CreateMap<DeviceLogAddDto, DeviceLogModel>();
            CreateMap<DeviceLogModel, DeviceLogDto>();
            //设备统计数据

            //设备历史数据

            //设备图片
            CreateMap<DeviceImageAddDto, DeviceImageModel>();
            CreateMap<DeviceImageUpdateDto, DeviceImageModel>();
            CreateMap<DeviceImageModel, DeviceImageDto>();

            //设备视频
            CreateMap<DeviceVideoAddDto, DeviceVideoModel>();
            CreateMap<DeviceVideoUpdateDto, DeviceVideoModel>();
            CreateMap<DeviceVideoModel, DeviceVideoDto>();
            //设备配置
            CreateMap<DeviceConfigAddDto, DeviceConfigModel>();
            CreateMap<DeviceConfigUpdateDto, DeviceConfigModel>();
            CreateMap<DeviceConfigModel, DeviceConfigDto>();
            //设备PLC配置数据
            CreateMap<DeviceHardwareConfigAddDto, DeviceHardwareConfigModel>();
            CreateMap<DeviceHardwareConfigUpdateDto, DeviceHardwareConfigModel>();
            CreateMap<DeviceHardwareConfigModel, DeviceHardwareConfigDto>();
            CreateMap<TypeHardwareConfigModel, DeviceHardwareConfigModel>().ForMember(dest => dest.Id, opt => opt.Ignore()).ForMember(dest => dest.Create, opt => opt.Ignore()).ForMember(dest => dest.CreateTime,
                opt => opt.Ignore()).ForMember(dest => dest.Modify, opt => opt.Ignore()).ForMember(dest => dest.ModifyTime, opt => opt.Ignore());
            //设备输入数据
            CreateMap<DeviceInputAddDto, DeviceInputDataModel>();
            CreateMap<DeviceInputDataUpdateDto, DeviceInputDataModel>();
            CreateMap<DeviceInputDataModel, DeviceInputDto>();
            //设备迁移数据
            CreateMap<DeviceMigrationModel, DeviceMigrationDto>();
            //设备在线数据
        }
    }
}
