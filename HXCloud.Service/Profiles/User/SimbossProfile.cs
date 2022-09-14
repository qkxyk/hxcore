using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public class SimbossProfile:Profile
    {
        public SimbossProfile()
        {
            CreateMap<SimbossModel, SimbossDto>();
            CreateMap<SimbossAddDto, SimbossModel>();
            CreateMap<SimbossUpdateDto, SimbossModel>();
        }
    }
}
