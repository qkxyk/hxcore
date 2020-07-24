using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<ProjectAddDto, ProjectModel>().ForMember(dest => dest.ProjectType, opt =>
               opt.MapFrom(src => (ProjectType)src.ProjectType));
            CreateMap<ProjectModel, ProjectData>().ForMember(dest => dest.Child, opt => opt.Ignore());
        }
    }
}
