using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupAddViewModel, GroupModel>().ForMember(a => a.GroupName, s => s.MapFrom(d => d.Name)).ForMember(
                a => a.GroupCode, s => s.MapFrom(d => d.Code)).ForMember(d => d.Description, s => s.MapFrom(a => a.Description));

            CreateMap<GroupModel, GroupData>().ForMember(d => d.Name, a => a.MapFrom(s => s.GroupName)).ForMember(d => d.Code, a => a.MapFrom(
                           s => s.GroupCode));

            CreateMap<GroupModel, GroupViewModel>().ForMember(d => d.GroupId, a => a.MapFrom(s => s.Id)).ForMember(
                d => d.GroupName, a => a.MapFrom(s => s.GroupName)).ForMember(d => d.GroupCode, a => a.MapFrom(s => s.GroupCode)).ForMember(
                d => d.Logo, a => a.MapFrom(s => s.Logo));

            CreateMap<GroupUpdateViewModel, GroupModel>().ForMember(d => d.Id, a => a.MapFrom(s => s.GroupId));

            CreateMap<AppVersionAddDto, AppVersionModel>();
            CreateMap<AppVersionUpdateDto, AppVersionModel>();
            CreateMap<AppVersionModel, AppVersionDto>();
        }
    }
}
