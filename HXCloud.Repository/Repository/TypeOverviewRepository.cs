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
    public class TypeOverviewRepository : BaseRepository<TypeOverviewModel>, ITypeOverviewRepository
    {
        public async Task<IEnumerable<TypeOverviewModel>> FindWithDataDefine(Expression<Func<TypeOverviewModel, bool>> predicate)
        {
            var data = await _db.TypeOverviews.Include(a => a.TypeDataDefine).Where(predicate).ToListAsync();
            return data;
        }
    }
}
