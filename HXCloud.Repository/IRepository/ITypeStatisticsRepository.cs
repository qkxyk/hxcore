using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeStatisticsRepository : IBaseRepository<TypeStatisticsInfoModel>
    {
        Task<TypeStatisticsInfoModel> FindWithType(Expression<Func<TypeStatisticsInfoModel, bool>> predicate);
    }
}
