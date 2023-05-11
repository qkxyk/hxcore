using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IAggregateRoot
    {
        public HXCloudContext _db { get; set; }//属性注入,注入会自动调用dispose方法
        //protected HXCloudContext _db;
        //public BaseRepository(HXCloudContext context)
        //{
        //    _db = context;
        //}

        public void Add(T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
        }

        public async virtual Task AddAsync(T entity)
        {
            _db.Set<T>().Add(entity);
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public T Find(params object[] Ids)
        {
            var ret = _db.Set<T>().Find(Ids);
            return ret;
        }

        public virtual async Task<T> FindAsync(params object[] Ids)
        {
            var ret = await _db.Set<T>().FindAsync(Ids);
            return ret;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> lambda)
        {
            var ret = _db.Set<T>().AsNoTracking().Where(lambda);
            return ret;
        }

        public void Remove(T entity)
        {
            _db.Set<T>().Remove(entity);
            _db.SaveChanges();
        }

        public async virtual Task RemoveAsync(T entity)
        {
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        public void Save(T entity)
        {
            _db.Entry<T>(entity).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public async Task SaveAsync(T entity)
        {
            _db.Entry<T>(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}
