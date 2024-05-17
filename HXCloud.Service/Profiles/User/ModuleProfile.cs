using AutoMapper;
using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Service
{
    public class ModuleProfile: Profile
    {
        public ModuleProfile()
        {
            CreateMap<ModuleAddDto, ModuleModel>();
            CreateMap<ModuleModel, ModuleDto>();

            CreateMap<ModuleOperateAddDto, ModuleOperateModel>();
            CreateMap<ModuleOperateModel, ModuleOperateDto>();

            CreateMap<RoleModuleOperateAddDto, RoleModuleOperateModel>();
            CreateMap<RoleModuleOperateModel, RoleModuleOperateDto>();
        }
    }
}
