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
    public class TypeArgumentRepository : BaseRepository<TypeArgumentModel>, ITypeArgumentRepository
    {
        public async Task<TypeArgumentModel> FindWithType(Expression<Func<TypeArgumentModel, bool>> predicate)
        {
            var data = await _db.TypeArguments.Include(a => a.Type).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
