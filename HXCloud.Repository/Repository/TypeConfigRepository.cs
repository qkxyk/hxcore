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
    public class TypeConfigRepository : BaseRepository<TypeConfigModel>, ITypeConfigRepository
    {
        public async Task<TypeConfigModel> FindWithType(Expression<Func<TypeConfigModel, bool>> predicate)
        {
            var data = await _db.TypeConfigs.Include(a => a.Type).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
