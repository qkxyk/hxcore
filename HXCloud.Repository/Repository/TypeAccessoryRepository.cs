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
    public class TypeAccessoryRepository : BaseRepository<TypeAccessoryModel>, ITypeAccessoryRepository
    {
        public async Task<TypeAccessoryModel> FindWithType(Expression<Func<TypeAccessoryModel, bool>> predicate)
        {
            var data = await _db.TypeAccessories.Include(a => a.Type).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
        public async Task<IEnumerable<TypeAccessoryModel>> FindWithControlData(int typeId)
        {
            var data = await _db.TypeAccessories.Include(a => a.TypeAccessoryControlDatas).ThenInclude(a => a.TypeDataDefine).Where(a => a.TypeId == typeId).ToListAsync();
            return data;
        }
    }
}
