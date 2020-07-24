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
    public class TypeStatisticsRepository : BaseRepository<TypeStatisticsInfoModel>, ITypeStatisticsRepository
    {
        public async Task<TypeStatisticsInfoModel> FindWithType(Expression<Func<TypeStatisticsInfoModel, bool>> predicate)
        {
            var data = await _db.TypeStatisticsInfos.Include(a => a.Type).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
