using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class RoleProjectRepository : BaseRepository<RoleProjectModel>, IRoleProjectRepository
    {
        public async Task SaveAsync(int RoleId, List<RoleProjectModel> rp)
        {
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    //先删除角色分配的项目
                    var list = await _db.RoleProjects.Where(a => a.RoleId == RoleId).ToListAsync();
                    _db.RoleProjects.RemoveRange(list);
                    //添加角色项目
                    await _db.RoleProjects.AddRangeAsync(rp);
                    await _db.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }

            }
        }
    }
}
