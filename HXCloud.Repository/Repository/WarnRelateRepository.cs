using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public class WarnTypeRepository : BaseRepository<WarnTypeModel>, IWarnTypeRepository
    {
        public async Task<WarnTypeModel> FindWithCodeAsync(Expression<Func<WarnTypeModel, bool>> predicate)
        {
            var data = await _db.WarnTypes.Include(a => a.WarnCode).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
        public async Task<IEnumerable<WarnTypeModel>> FindTypesWithCodeAsync(Expression<Func<WarnTypeModel, bool>> predicate)
        {
            var data = await _db.WarnTypes.Include(a => a.WarnCode).Where(predicate).ToListAsync();
            return data;
        }
    }

    public class WarnCodeRepository : BaseRepository<WarnCodeModel>, IWarnCodeRepository
    {

    }
    public class WarnRepository : BaseRepository<WarnModel>, IWarnRepository
    {

        public IQueryable<WarnModel> FindWithCodeAndType(Expression<Func<WarnModel,bool>> predicate)
        {
            var data = _db.Warns.Include(a => a.WarnCode).ThenInclude(a => a.WarnType).Where(predicate);
            return data;
        }
    }
}
