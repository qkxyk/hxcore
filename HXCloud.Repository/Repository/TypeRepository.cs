using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class TypeRepository : BaseRepository<TypeModel>, ITypeRepository
    {
        /// <summary>
        /// 获取类型和类型的子节点
        /// </summary>
        /// <param name="predicate">类型表达式</param>
        /// <returns>返回满足条件的第一个类型</returns>
        public async Task<TypeModel> FindAsync(Expression<Func<TypeModel, bool>> predicate)
        {
            var data = await _db.Types.Include(a => a.Child).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
