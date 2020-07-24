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
    public class TypeHardwareConfigRepository : BaseRepository<TypeHardwareConfigModel>, ITypeHardwareConfigRepository
    {
        public async Task<TypeHardwareConfigModel> FindWithType(Expression<Func<TypeHardwareConfigModel, bool>> predicate)
        {
            var data = await _db.TypeHardwareConfigs.Include(a => a.Type).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
