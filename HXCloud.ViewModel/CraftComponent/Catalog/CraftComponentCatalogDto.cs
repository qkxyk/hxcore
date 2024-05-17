using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class CraftComponentCatalogDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }//组件类型图标
        public ICollection<CraftElementDto> CraftElements { get; set; }
        public ICollection<CraftComponentCatalogDto> Child { get; set; }
    }
}
