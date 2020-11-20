using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IProjectRepository : IBaseRepository<ProjectModel>
    {
        IQueryable<ProjectModel> FindWithImageAndChildAsync(Expression<Func<ProjectModel, bool>> predicate);
        Task<ProjectModel> FindWithChildAsync(Expression<Func<ProjectModel, bool>> predicate);
        IQueryable<ProjectModel> FindWithImageAndDeviceAsync(Expression<Func<ProjectModel, bool>> predicate);
        IQueryable<ProjectModel> FindProjectsWithImageByParentAsync(Expression<Func<ProjectModel, bool>> predicate);
    }
}
