using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class WarnTypeModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

        public string Color { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<WarnCodeModel> WarnCode { get; set; }

        //public string GroupId { get; set; }
        //public virtual GroupModel Group { get; set; }
    }
}
