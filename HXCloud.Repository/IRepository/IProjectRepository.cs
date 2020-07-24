using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IProjectRepository:IBaseRepository<ProjectModel>
    {
        Task<ProjectModel> FindAsync(Expression<Func<ProjectModel, bool>> predicate);
    }
}
