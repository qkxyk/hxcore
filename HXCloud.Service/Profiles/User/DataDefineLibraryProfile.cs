using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public class DataDefineLibraryProfile:Profile
    {
        public DataDefineLibraryProfile()
        {
            CreateMap<DataDefineLibraryAddDto, DataDefineLibraryModel>();
            CreateMap<DataDefineLibraryUpdateDto, DataDefineLibraryModel>().ForMember(dest=>dest.ModifyTime,opt=>opt.MapFrom(src=>DateTime.Now));
            CreateMap<DataDefineLibraryModel, DataDefineLibraryDataDto>();
        }
    }
}
