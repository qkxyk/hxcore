using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleAddDto, RoleModel>().ForMember(d => d.RoleName, a => a.MapFrom(s => s.Name));
            CreateMap<RoleModel, RoleDataDto>().ForMember(d => d.Name, a => a.MapFrom(s => s.RoleName))
                .ForMember(d => d.Admin, a => a.MapFrom(s => s.IsAdmin ? "是" : "否"));

            CreateMap<RoleUpdateDto, RoleModel>().ForMember(d => d.RoleName, a => a.MapFrom(s => s.Name)).ForMember(
                d => d.ModifyTime, a => a.MapFrom(s => DateTime.Now));
            CreateMap<RoleProjectModel, RoleProjectDto>().ForMember(dest => dest.Operate, opt => opt.MapFrom(src => (int)src.Operate));
        }
    }
}
