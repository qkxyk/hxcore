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
            CreateMap<DeviceAddDto, DeviceModel>().ForMember(dest => dest.Water, opt => opt.MapFrom(src => (DeviceWater)src.Water));
            CreateMap<DeviceUpdateViewModel, DeviceModel>().ForMember(dest => dest.Water, opt => opt.MapFrom(src => (DeviceWater)src.Water));
            CreateMap<DeviceModel, DeviceDataDto>().ForMember(dest => dest.OnLine, opt => opt.MapFrom(src => src.DeviceOnline == null ? false : src.DeviceOnline.State))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.DeviceImage)).ForMember(dest => dest.Water, opt => opt.MapFrom(src => (int)src.Water));
            //设备patch数据
            CreateMap<DevicePatchDto, DeviceModel>();
            CreateMap<DeviceModel, DevicePatchDto>();

            //设备流量卡
            CreateMap<DeviceCardAddDto, DeviceCardModel>();
            CreateMap<DeviceCardUpdateDto, DeviceCardModel>();
            CreateMap<DeviceCardPositionUpdateDto, DeviceCardModel>();
            CreateMap<DeviceCardModel, DeviceCardDto>();
            //设备操作日志
            CreateMap<DeviceLogAddDto, DeviceLogModel>();
            CreateMap<DeviceLogModel, DeviceLogDto>();
            //设备统计数据
            CreateMap<DeviceStatisticsDataModel, DeviceStatisticsDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Device.TypeId)).ForMember(dest => dest.DeviceName,
                opt => opt.MapFrom(src => src.Device.DeviceName));
            CreateMap<DeviceDiscreteStatisticsDataModel, DeviceStatisticsDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Device.TypeId)).ForMember(dest => dest.DeviceName,
               opt => opt.MapFrom(src => src.Device.DeviceName));

            //设备历史数据
            //CreateMap<DeviceHisDataModel, DeviceHisDataDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Device.TypeId)).ForMember(dest => dest.DeviceName,
            //    opt => opt.MapFrom(src => src.Device.DeviceName)).ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.Device.ProjectId)).ForMember(dest => dest.DeviceNo,
            //    opt => opt.MapFrom(src => src.Device.DeviceNo));
            CreateMap<CoreDeviceHisDataMoel, DeviceHisDataDto>();

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

            //设备集采仪数据
            CreateMap<DeviceMonitorDataModel, DeviceMonitorDto>().ForMember(dest => dest.WaterType, opt => opt.MapFrom(src => src.WaterType.ToString()));

            //设备集采仪数据
            CreateMap<DeviceDayMonitorDataModel, DeviceMonitorDto>().ForMember(dest => dest.WaterType, opt => opt.MapFrom(src => src.WaterType.ToString()));
        }
    }
}
