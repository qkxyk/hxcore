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
    public class TypeAccessoryControlDataRepository : BaseRepository<TypeAccessoryControlDataModel>, ITypeAccessoryControlDataRepository
    {
        public async Task<TypeAccessoryControlDataModel> FindWithType(Expression<Func<TypeAccessoryControlDataModel, bool>> predicate)
        {
            var data = await _db.TypeAccessoryControlDatas.Include(a => a.TypeAccessory).ThenInclude(a => a.Type).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
