using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository.Repository
{
    public class GroupRepository : BaseRepository<GroupModel>, IGroupRepository
    {
        public async Task Add(GroupModel entity, UserModel user)
        {
            _db.Groups.Add(entity);
            _db.Users.Add(user);
            //创建角色，并给用户赋该角色
            RoleModel rm = new RoleModel() { GroupId = entity.Id, IsAdmin = true, RoleName = "Admin", Create = entity.Create };
            UserRoleModel ur = new UserRoleModel() { User = user, Role = rm };
            _db.Roles.Add(rm);
            _db.UserRoles.Add(ur);
            await _db.SaveChangesAsync();
            return;
        }
    }
}
