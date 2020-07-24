using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeSystemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; } = 1;//备用，用于子系统的排名

        public int TypeId { get; set; }
    }
}
