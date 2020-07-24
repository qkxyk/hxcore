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
    public class TypeDataDefineRepository : BaseRepository<TypeDataDefineModel>, ITypeDataDefineRepository
    {
        public async Task<TypeDataDefineModel> GetTypeDataDefineWithTypeAsync(Expression<Func<TypeDataDefineModel, bool>> predicate)
        {
            var data = await _db.TypeDataDefines.Include(a => a.Type).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
