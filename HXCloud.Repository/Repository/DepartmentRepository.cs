using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class DepartmentRepository : BaseRepository<DepartmentModel>, IDepartmentRepository
    {
        public async Task<DepartmentModel> FindAsync(Expression<Func<DepartmentModel,bool>> predicate)
        {
            var data = await _db.Departments.Include(a => a.Child).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
