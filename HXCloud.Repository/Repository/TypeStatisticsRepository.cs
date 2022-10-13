﻿using System;
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

        public async Task<IEnumerable<TypeStatisticsInfoModel>> FindGlobalStaticsBySql(int showState)
        {
            var d = await _db.TypeStatisticsInfos.FromSqlRaw($"select * from TypeStatisticsInfo where id in(SELECT   max(id)    FROM [hxnetcore].[dbo].[TypeStatisticsInfo] where ShowState={showState} group by DataKey) order by DataKey").ToListAsync();
            return d;
        }
    }
}
