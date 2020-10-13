using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;
using Microsoft.Data.SqlClient;

namespace HXCloud.Service
{
    public class WarnProfile : Profile
    {
        public WarnProfile()
        {
            #region 报警类型
            CreateMap<WarnTypeAddDto, WarnTypeModel>();
            CreateMap<WarnTypeUpdateDto, WarnTypeModel>();
            CreateMap<WarnTypeModel, WarnTypeDto>();
            #endregion
            #region 报警代码
            CreateMap<WarnCodeAddDto, WarnCodeModel>();
            CreateMap<WarnCodeUpdateDto, WarnCodeModel>();
            CreateMap<WarnCodeModel, WarnCodeDto>().ForMember(dest => dest.WarnTypeName, opt => opt.MapFrom(src => src.WarnType.TypeName));
            #endregion
            #region 报警数据
            CreateMap<WarnModel, WarnDto>().ForMember(dest => dest.WarnTypeId, opt => opt.MapFrom(src => src.WarnCode.WarnType.Id)).ForMember(
                dest => dest.WarnTypeName, opt => opt.MapFrom(src => src.WarnCode.WarnType.TypeName)).ForMember(dest => dest.Code, opt => opt.MapFrom(
                              src => src.WarnCode.Code)).ForMember(dest => dest.CodeDescription, opt => opt.MapFrom(src => src.WarnCode.Description)).
                              ForMember(dest => dest.DeviceName, opt => opt.MapFrom(src => src.Device.DeviceName)).ForMember(dest => dest.FullId, opt => opt.MapFrom(
                                            src => src.Device.FullId)).ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Device.FullName));
            CreateMap<WarnUpdateDto, WarnModel>();
            #endregion
        }
    }
}
