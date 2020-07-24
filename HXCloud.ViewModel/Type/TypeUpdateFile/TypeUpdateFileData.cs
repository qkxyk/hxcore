using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeUpdateFileData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Version { get; set; }

        public string Description { get; set; }

        public int TypeId { get; set; }
    }
}
