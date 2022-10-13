using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Service
{
    public class OpsProfile : Profile
    {
        public OpsProfile()
        {
            //巡检项目
            CreateMap<OpsItemAddDto, OpsItemModel>().ForMember(dest => dest.OpsType, opt => opt.MapFrom(src => (OpsType)src.OpsType));
            CreateMap<OpsItemUpdateDto, OpsItemModel>().ForMember(dest => dest.OpsType, opt => opt.MapFrom(src => (OpsType)src.OpsType));
            CreateMap<OpsItemModel, OpsItemDto>().ForMember(dest => dest.OpsType, opt => opt.MapFrom(src => (int)src.OpsType));
            //类型巡检项目
            CreateMap<TypeOpsItemAddDto, TypeOpsItemModel>().ForMember(dest => dest.OpsType, opt => opt.MapFrom(src => (OpsType)src.OpsType));
            CreateMap<TypeOpsItemUpdateDto, TypeOpsItemModel>().ForMember(dest => dest.OpsType, opt => opt.MapFrom(src => (OpsType)src.OpsType));
            CreateMap<TypeOpsItemModel, TypeOpsItemDto>().ForMember(dest => dest.OpsType, opt => opt.MapFrom(src => (int)src.OpsType));
            //巡检数据
            CreateMap<PatrolDataAddDto, PatrolDataModel>();
            //巡检图片
            CreateMap<PatrolImageAddDto, PatrolImageModel>();
            //巡检项目转换类型巡检数据
            CreateMap<OpsItemModel, TypeOpsItemDto>();
            CreateMap<TypeOpsItemModel, TypeOpsItemDto>();
            CreateMap<PatrolDataModel, PatrolDataDto>().ForMember(dest => dest.PatrolImage, opt => opt.MapFrom(src => src.PatrolImage.Url))
                .ForMember(dest => dest.ProductData, opt => opt.MapFrom(src => src.ProductData.Content)).ForMember(dest => dest.WaterAnalysis,
                opt =>opt.MapFrom(src => src.WaterAnalysis.Content)).ForMember(dest => dest.DevicePatrol, opt => opt.MapFrom(src => src.DevicePatrol.Content))
                .ForMember(dest => dest.TechniquePatrol, opt => opt.MapFrom(src => src.TechniquePatrol.Content));
            //问题单
            CreateMap<IssueAddDto, IssueModel>();
            CreateMap<IssueUpdateDto, IssueModel>();
            CreateMap<IssueModel, IssueDto>();
            //维修
            CreateMap<RepairAddDto, RepairModel>().ForMember(dest => dest.RepairType, opt => opt.MapFrom(src => (RepairType)src.RepairType)).ForMember(
                dest=>dest.EmergenceStatus,opt=>opt.MapFrom(src=>(EmergenceStatus)src.EmergenceStatus));
            CreateMap<RepairModel, RepairDto>();
            CreateMap<RepairModel, RepairAndIssueDto>();
        }
    }
}
