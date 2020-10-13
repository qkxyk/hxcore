using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public interface IWarnTypeRepository : IBaseRepository<WarnTypeModel>
    {
        Task<WarnTypeModel> FindWithCodeAsync(Expression<Func<WarnTypeModel, bool>> predicate);
        Task<IEnumerable<WarnTypeModel>> FindTypesWithCodeAsync(Expression<Func<WarnTypeModel, bool>> predicate);
    }

    public interface IWarnCodeRepository : IBaseRepository<WarnCodeModel>
    {
        IQueryable<WarnCodeModel> FindWithWarnType(Expression<Func<WarnCodeModel, bool>> predicate);
    }
    public interface IWarnRepository : IBaseRepository<WarnModel>
    {
        IQueryable<WarnModel> FindWithCodeAndType(Expression<Func<WarnModel, bool>> predicate);
    }
}
