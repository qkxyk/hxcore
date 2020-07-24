using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class UserDepartmentModel : BaseCModel/* BaseModel, IAggregateRoot*/
    {
        public int UserId { get; set; }
        public int DeparmentId { get; set; }

        public virtual UserModel User { get; set; }
        public virtual DepartmentModel Department { get; set; }
    }
}
