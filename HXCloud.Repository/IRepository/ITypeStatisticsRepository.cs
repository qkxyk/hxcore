using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeStatisticsRepository : IBaseRepository<TypeStatisticsInfoModel>
    {
        Task<TypeStatisticsInfoModel> FindWithType(Expression<Func<TypeStatisticsInfoModel, bool>> predicate);
        Task<IEnumerable<TypeStatisticsInfoModel>> FindGlobalStaticsBySql(int showState);
        /// <summary>
        /// 关联类型数据定义的计算公式
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TypeStatisticsInfoModel> FindWithFormat(Expression<Func<TypeStatisticsInfoModel, bool>> predicate);
    }
}
