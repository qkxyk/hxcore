using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Service
{
    public interface IBaseService<T> where T : class, IAggregateRoot
    {
        Task<bool> IsExist(Expression<Func<T, bool>> predicate);
    }
    public interface IService<T, U> : IBaseService<T>
        where T : class, IAggregateRoot
        where U : class
    {
        Task<U> IsExistCheck(Expression<Func<T, bool>> predicate);
    }
}
