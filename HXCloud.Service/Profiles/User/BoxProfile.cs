using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Service
{
    public class BoxProfile : Profile
    {
        public BoxProfile()
        {
            //设备
            CreateMap<BoxAddDto, BoxModel>();
            CreateMap<BoxModel, BoxDto>();
        }
    }
}
