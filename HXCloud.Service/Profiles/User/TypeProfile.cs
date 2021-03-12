using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;
using Microsoft.Data.SqlClient;

namespace HXCloud.Service
{
    public class TypeProfile : Profile
    {
        public TypeProfile()
        {
            CreateMap<TypeAddViewModel, TypeModel>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => (TypeStatus)src.Status));
            CreateMap<TypeUpdateViewModel, TypeModel>();
            CreateMap<TypeModel, TypeData>().ForMember(dest => dest.Child, opt => opt.Ignore()/* opt.MapFrom(src => src.Child)*/).ForMember(
                dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status));
            //类型图片
            CreateMap<TypeImageAddViewModel, TypeImageModel>();
            CreateMap<TypeImageUpdateViewModel, TypeImageModel>();
            CreateMap<TypeImageModel, TypeImageData>();
            //类型更新文件
            CreateMap<TypeUpdateFileAddViewModel, TypeUpdateFileModel>();
            CreateMap<TypeUpdateFileUpdateViewModel, TypeUpdateFileModel>();
            CreateMap<TypeUpdateFileModel, TypeUpdateFileData>();
            //类型数据定义
            CreateMap<TypeDataDefineAddViewModel, TypeDataDefineModel>();
            CreateMap<TypeDataDefineUpdateViewModel, TypeDataDefineModel>();
            CreateMap<TypeDataDefineModel, TypeDataDefineData>();
            //类型模式
            CreateMap<TypeSchemaAddViewModel, TypeSchemaModel>().ForMember(dest => dest.SchemaType, opt => opt.MapFrom(src => (SchemaType)src.SchemaType));
            CreateMap<TypeSchemaModel, TypeSchemaData>().ForMember(dest => dest.Child, opt => opt.Ignore()).ForMember(dest => dest.SchemaType, opt => opt.MapFrom(
                   src => (int)src.SchemaType));
            CreateMap<TypeSchemaUpdateViewModel, TypeSchemaModel>();
            //类型配置
            CreateMap<TypeConfigAddViewModel, TypeConfigModel>();
            CreateMap<TypeConfigUpdateViewModel, TypeConfigModel>();
            CreateMap<TypeConfigModel, TypeConfigData>();
            //类型统计
            CreateMap<TypeStatisticsAddViewModel, TypeStatisticsInfoModel>().ForMember(dest => dest.StaticsType, opt => opt.MapFrom(
                 src => (StatisticsType)src.StaticsType));
            CreateMap<TypeStatisticsUpdateViewModel, TypeStatisticsInfoModel>().ForMember(dest => dest.StaticsType, opt => opt.MapFrom(
                   src => (StatisticsType)src.StaticsType));
            CreateMap<TypeStatisticsInfoModel, TypeStatisticsData>().ForMember(opt => opt.StaticsType, opt => opt.MapFrom(src => (int)src.StaticsType));
            /*
            //类型配件
            CreateMap<TypeAccessoryAddViewModel, TypeAccessoryModel>();
            CreateMap<TypeAccessoryUpdateViewModel, TypeAccessoryModel>();
            CreateMap<TypeAccessoryModel, TypeAccessoryData>();

            CreateMap<TypeAccessoryModel, TypeAccessoryDto>().ForMember(dest => dest.Controls, opt => opt.MapFrom(src => src.TypeAccessoryControlDatas));
            CreateMap<TypeAccessoryControlDataModel, AccessoryControlDto>().ForMember(dest => dest.DataKey, opt => opt.MapFrom(src => src.TypeDataDefine.DataKey)).ForMember(
                dest => dest.DataType, opt => opt.MapFrom(src => src.TypeDataDefine.DataType)).ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.TypeDataDefine.Unit)).ForMember(
                dest => dest.Format, opt => opt.MapFrom(src => src.TypeDataDefine.Format)).ForMember(dest => dest.AutoControl, opt => opt.MapFrom(src => src.TypeDataDefine.AutoControl));
            //配件控制项
            CreateMap<TypeControlDataAddDto, TypeAccessoryControlDataModel>();
            CreateMap<TypeControlDataUpdateDto, TypeAccessoryControlDataModel>();
            CreateMap<TypeAccessoryControlDataModel, TypeControlDataDto>();
            */
            //类型参数
            CreateMap<TypeArgumentAddViewModel, TypeArgumentModel>();
            CreateMap<TypeArgumentUpdateViewModel, TypeArgumentModel>();
            CreateMap<TypeArgumentModel, TypeArgumentData>();
            CreateMap<TypeArgumentModel, TypeArgumentDto>().ForMember(dest => dest.DefineKey, opt => opt.MapFrom(src => src.TypeDataDefine.DataKey)).ForMember(dest => dest.DefineType, opt => opt.MapFrom(
                           src => src.TypeDataDefine.ShowType)).ForMember(dest => dest.DefineFormat, opt => opt.MapFrom(src => src.TypeDataDefine.Format)).ForMember(dest => dest.DefineUnit, opt => opt.MapFrom(
                                       src => src.TypeDataDefine.Unit));
            /*
             //类型子系统
             CreateMap<TypeSystemAddDto, TypeSystemModel>();
             CreateMap<TypeSystemUpdateDto, TypeSystemModel>();
             CreateMap<TypeSystemModel, TypeSystemDto>();
             //子系统配件
             CreateMap<TypeSystemAccessoryAddDto, TypeSystemAccessoryModel>();
             CreateMap<TypeSystemAccessoryUpdateDto, TypeSystemAccessoryModel>();
             CreateMap<TypeSystemAccessoryModel, TypeSystemAccessoryDto>();
             //子系统配件控制项
             CreateMap<TypeSystemAccessoryControlDataAddDto, TypeSystemAccessoryControlDataModel>();
             CreateMap<TypeSystemAccessoryControlDataUpdateDto, TypeSystemAccessoryControlDataModel>();
             CreateMap<TypeSystemAccessoryControlDataModel, TypeSystemAccessoryControlDataDto>();
            */
            //类型硬件配置
            CreateMap<TypeHardwareConfigAddDto, TypeHardwareConfigModel>();
            CreateMap<TypeHardwareConfigUpdateDto, TypeHardwareConfigModel>();
            CreateMap<TypeHardwareConfigModel, TypeHardwareConfigDto>();
            //类型总揽
            CreateMap<TypeOverViewAddDto, TypeOverviewModel>();
            CreateMap<TypeOverviewUpdateDto, TypeOverviewModel>();
            CreateMap<TypeOverviewModel, TypeOverviewDto>().ForMember(dest => dest.DataDefineKey, opt => opt.MapFrom(src => src.TypeDataDefine.DataKey)).
            ForMember(dest => dest.ShowType, opt => opt.MapFrom(src => src.TypeDataDefine.ShowType)).ForMember(dest => dest.DefaultValue, opt => opt.MapFrom(
                          src => src.TypeDataDefine.DefaultValue)).ForMember(dest => dest.Format, opt => opt.MapFrom(src => src.TypeDataDefine.Format)).ForMember(
                dest => dest.Unit, opt => opt.MapFrom(src => src.TypeDataDefine.Unit));
            //类型显示图标
            CreateMap<TypeDisplayIconAddDto, TypeDisplayIconModel>();
            CreateMap<TypeDisplayIconUpdateDto, TypeDisplayIconModel>();
            CreateMap<TypeDisplayIconModel, TypeDisplayIconDto>().ForMember(dest => dest.DataDefineKey, opt => opt.MapFrom(src => src.TypeDataDefine.DataKey));
            //类型模块
            CreateMap<TypeModuleAddDto, TypeModuleModel>().ForMember(dest => dest.ModuleType, opt => opt.MapFrom(src => (ModuleType)src.ModuleType));
            CreateMap<TypeModuleUpdateDto, TypeModuleModel>().ForMember(dest => dest.ModuleType, opt => opt.MapFrom(src => (ModuleType)src.ModuleType));
            CreateMap<TypeModuleModel, TypeModuleDto>().ForMember(dest => dest.Controls, opt => opt.MapFrom(src => src.ModuleControls))/*.ForMember(dest => dest.Arguments,
                opt => opt.MapFrom(src => src.ModeleArguments))*/;

            //类型控制项
            CreateMap<TypeModuleControlAddDto, TypeModuleControlModel>();
            CreateMap<TypeModuleControlUpdateDto, TypeModuleControlModel>();
            CreateMap<TypeModuleControlModel, TypeModuleControlDto>().ForMember(dest => dest.Format, opt => opt.MapFrom(src => src.TypeDataDefine.Format))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.TypeDataDefine.Unit)).ForMember(dest => dest.ShowType, opt => opt.MapFrom(src => src.TypeDataDefine.ShowType))
                .ForMember(dest => dest.DefaultValue, opt => opt.MapFrom(src => src.TypeDataDefine.DefaultValue)).ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.TypeDataDefine.DataKey))
                .ForMember(dest => dest.FeedBacks, opt => opt.MapFrom(src => src.TypeModuleFeedbacks)).ForMember(dest => dest.AutoControl, opt => opt.MapFrom(src => src.TypeDataDefine.AutoControl))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.TypeClass.Name)).ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.TypeClass.Rank));
            //类型反馈数据
            CreateMap<TypeModuleFeedbackAddDto, TypeModuleFeedbackModel>();
            CreateMap<TypeModuleFeedbackUpdateDto, TypeModuleFeedbackModel>();
            CreateMap<TypeModuleFeedbackModel, TypeModuleFeedbackDto>().ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.TypeDataDefine.DataKey)).ForMember(dest => dest.Format, opt =>
                      opt.MapFrom(src => src.TypeDataDefine.Format)).ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.TypeDataDefine.Unit)).ForMember(dest => dest.ShowType, opt =>
                      opt.MapFrom(src => src.TypeDataDefine.ShowType)).ForMember(dest => dest.DefaultValue, opt => opt.MapFrom(src => src.TypeDataDefine.DefaultValue));
            //类型分组
            CreateMap<TypeClassModel, TypeClassDto>();
            CreateMap<TypeClassAddDto, TypeClassModel>();
            CreateMap<TypeClassUpdateDto, TypeClassModel>();
            CreateMap<TypeModuleArgumentAddDto, TypeModuleArgumentModel>();
            CreateMap<TypeModuleArgumentUpdateDto, TypeModuleArgumentModel>();
            CreateMap<TypeModuleArgumentModel, TypeModuleArgumentDto>().ForMember(dest => dest.DataKey, opt => opt.MapFrom(src => src.TypeDataDefine.DataKey)).ForMember(dest => dest.DataName, opt => opt.MapFrom(
                           src => src.TypeDataDefine.DataName)).ForMember(dest => dest.DefineFormat, opt => opt.MapFrom(src => src.TypeDataDefine.Format));
        }
    }
}
