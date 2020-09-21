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
    public class TypeDisplayIconRepository : BaseRepository<TypeDisplayIconModel>, ITypeDisplayIconRepository
    {
        public async Task<IEnumerable<TypeDisplayIconModel>> FindWithDataDefine(Expression<Func<TypeDisplayIconModel, bool>> predicate)
        {
            var data = await _db.TypeDisplayIcons.Include(a => a.TypeDataDefine).Where(predicate).ToListAsync();
            return data;
        }
    }
}
