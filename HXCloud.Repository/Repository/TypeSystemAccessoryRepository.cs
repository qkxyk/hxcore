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
    public class TypeSystemAccessoryRepository : BaseRepository<TypeSystemAccessoryModel>, ITypeSystemAccessoryRepository
    {

        public async Task<TypeSystemAccessoryModel> FindWithSystem(Expression<Func<TypeSystemAccessoryModel, bool>> predicate)
        {
            var data = await _db.TypeSystemAccessories.Include(a => a.TypeSystem).ThenInclude(a => a.Type).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
        public async Task<TypeSystemAccessoryModel> FindWithControlData(Expression<Func<TypeSystemAccessoryModel, bool>> predicate)
        {
            var data = await _db.TypeSystemAccessories.Include(a => a.TypeSystemAccessoryControlDatas).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
