using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class UserRoleModel : BaseCModel /*BaseModel, IAggregateRoot*/
    {
        public int UserId { get; set; }//用户标示
        public int RoleId { get; set; }//角色标示

        public virtual UserModel User { get; set; } //关联的用户
        public virtual RoleModel Role { get; set; }//关联的角色
    }
}
