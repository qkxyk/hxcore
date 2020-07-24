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
    public class TypeSystemRepository : BaseRepository<TypeSystemModel>, ITypeSystemRepository
    {
        public async Task<TypeSystemModel> FindWithType(Expression<Func<TypeSystemModel, bool>> predicate)
        {
            var data = await _db.TypeSystems.Include(a => a.Type).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
        public async Task<TypeSystemModel> FindWithAccessory(Expression<Func<TypeSystemModel, bool>> predicate)
        {
            var data = await _db.TypeSystems.Include(a => a.SystemAccessories).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
