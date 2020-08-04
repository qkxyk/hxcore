﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            CreateMap<ProjectModel, ProjectData>().ForMember(dest => dest.Child, opt => opt.Ignore()).ForMember(dest => dest.Image,
                opt => opt.MapFrom(src => src.Images.Count > 0 ? src.Images.OrderByDescending(a => a.Rank).FirstOrDefault().url : null));

            CreateMap<ProjectUpdateDto, ProjectModel>();
            CreateMap<ProjectImageAddDto, ProjectImageModel>();
            CreateMap<ProjectImageModel, ProjectImageData>();
        }
    }
}
