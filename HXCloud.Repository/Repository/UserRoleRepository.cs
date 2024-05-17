using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class UserRoleRepository : BaseRepository<UserRoleModel>, IUserRoleRepository
    {
        /// <summary>
        /// 根据用户标识获取用户的角色信息
        /// </summary>
        /// <param name="Id">用户标识</param>
        /// <returns></returns>
        public async Task<IEnumerable<UserRoleModel>> FindWithRole(int Id)
        {
            var rm = await _db.UserRoles.Include(a => a.Role).Where(a => a.UserId == Id).ToListAsync();
            return rm;
        }
        /// <summary>
        /// 更新用户角色信息
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="RoleIds">新增加的角色</param>
        /// <param name="account">操作人</param>
        /// <returns></returns>
        public async Task SaveAsync(int UserId, List<int> RoleIds, string account)
        {
            var role = await _db.UserRoles.Where(a => a.UserId == UserId).ToListAsync();
            _db.UserRoles.RemoveRange(role);
            foreach (var item in RoleIds)
            {
                _db.UserRoles.Add(new UserRoleModel { UserId = UserId, RoleId = item });
            }
            await _db.SaveChangesAsync();
        }
    }
}
