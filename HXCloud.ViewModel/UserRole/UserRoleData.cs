using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class UserRoleData
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
