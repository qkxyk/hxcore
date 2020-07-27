using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<RegionAddDto, RegionModel>().ForMember(dest => dest.GroupId, opt => opt.Ignore());
            CreateMap<RegionUpdateDto, RegionModel>().ForMember(dest => dest.GroupId, opt => opt.Ignore());
            CreateMap<RegionModel, RegionDto>().ForMember(dest => dest.Child, opt => opt.Ignore());
        }
    }
}
