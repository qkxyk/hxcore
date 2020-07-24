using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class ProjectRepository : BaseRepository<ProjectModel>, IProjectRepository
    {
        public async Task<ProjectModel> FindAsync(Expression<Func<ProjectModel, bool>> predicate)
        {
            var data = await _db.Projects.Include(a => a.Child).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
