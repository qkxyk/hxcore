using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public interface IBaseRepository<T> : IDisposable where T : IAggregateRoot
    {
        T Find(params object[] Ids);
        Task<T> FindAsync(params object[] Ids);
        void Add(T entity);
        void Save(T entity);
        void Remove(T entity);
        Task AddAsync(T entity);
        Task SaveAsync(T entity);
        Task RemoveAsync(T entity);
        IQueryable<T> Find(Expression<Func<T, bool>> lambda);
    }
}
