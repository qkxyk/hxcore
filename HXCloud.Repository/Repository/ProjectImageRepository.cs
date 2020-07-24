using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class ProjectImageRepository : BaseRepository<ProjectImageModel>, IProjectImageRepository
    {
        public async Task<ProjectImageModel> GetWithProject(int Id)
        {
            var data = await _db.ProjectImages.Include(a => a.project).Where(a => a.Id == Id).FirstOrDefaultAsync();
            return data;
        }
    }
}
