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
        public async Task<ProjectModel> FindWithChildAsync(Expression<Func<ProjectModel, bool>> predicate)
        {
            var data = await _db.Projects.Include(a => a.Child).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
        public IQueryable<ProjectModel> FindWithImageAndChildAsync(Expression<Func<ProjectModel, bool>> predicate)
        {
            var data = _db.Projects.Include(a => a.Child).Include(a => a.Images).Where(predicate);//.FirstOrDefaultAsync();
            return data;
        }

        public IQueryable<ProjectModel> FindWithImageAsync(Expression<Func<ProjectModel, bool>> predicate)
        {
            var data = _db.Projects.Include(a => a.Images).Where(predicate);//.FirstOrDefaultAsync();
            return data;
        }
    }
}
