using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IDepartmentRepository : IBaseRepository<DepartmentModel>
    {
        Task<DepartmentModel> FindAsync(Expression<Func<DepartmentModel, bool>> predicate);
    }
}
