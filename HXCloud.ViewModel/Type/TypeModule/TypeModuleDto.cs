﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeModuleDto
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public int ModuleType { get; set; } = 2;
        public int Sn { get; set; } = 0;//模块的先后顺序，主要用来排序从模块的先后顺序
        public TypeModuleDto()
        {
            Controls = new List<TypeModuleControlDto>();
            Arguments = new List<TypeModuleArgumentDto>();
        }
        public List<TypeModuleControlDto> Controls { get; set; }
        public IList<TypeModuleArgumentDto> Arguments { get; set; }
    }

    public class TypeModuleMockDto
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public int ModuleType { get; set; } = 2;
        public int Sn { get; set; } = 0;//模块的先后顺序，主要用来排序从模块的先后顺序
        public TypeModuleMockDto()
        {
            Controls = new List<TypeModuleControlMockDto>();
            //Arguments = new List<TypeModuleArgumentDto>();
        }
        public List<TypeModuleControlMockDto> Controls { get; set; }
        //public IList<TypeModuleArgumentDto> Arguments { get; set; }
    }
}
