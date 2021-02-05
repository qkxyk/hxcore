using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, UserMessage>().ForMember(s => s.GroupName, a => a.MapFrom(b => b.Group.GroupName)).ForMember(
              s => s.Code, a => a.MapFrom(src => src.Group.GroupCode)).ForMember(a => a.UserStatus, s => s.MapFrom(src => (int)src.Status));
            CreateMap<UserModel, UserLoginDto>().ForMember(s => s.GroupName, a => a.MapFrom(b => b.Group.GroupName)).ForMember(
                      s => s.Code, a => a.MapFrom(src => src.Group.GroupCode)).ForMember(a => a.UserStatus, s => s.MapFrom(src => (int)src.Status));
            CreateMap<UserRegisterViewModel, UserModel>();
            CreateMap<UserModel, UserData>().ForMember(d => d.Status, s => s.MapFrom(a => (int)a.Status)).ForMember(d => d.LastLogin, s => s.MapFrom(a => a.LastLogin == null ? null : a.LastLogin));
            CreateMap<UserModel, UserInfoDto>().ForMember(dest=>dest.Create,opt=>opt.MapFrom(src=>src.CreateTime)).ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.Group.Id)).ForMember(dest => dest.GroupName,
                opt => opt.MapFrom(src => src.Group.GroupName)).ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Group.GroupCode))
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Group.Logo));
            CreateMap<UserAddViewModel, UserModel>();
        }
    }
}
