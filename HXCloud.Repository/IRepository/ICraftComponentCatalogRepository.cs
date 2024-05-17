using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public interface ICraftComponentCatalogRepository : IBaseRepository<CraftComponentCatalogModle>
    {
        Task<List<CraftComponentCatalogModle>> GetAll(Expression<Func<CraftComponentCatalogModle, bool>> predicate);
        Task<CraftComponentCatalogModle> GetWithElementAsync(Expression<Func<CraftComponentCatalogModle, bool>> predicate);
    }
}
