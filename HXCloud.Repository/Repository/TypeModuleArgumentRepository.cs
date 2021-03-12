using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public class TypeModuleArgumentRepository : BaseRepository<TypeModuleArgumentModel>, ITypeModuleArgumentRepository
    {
        /// <summary>
        /// 查找模块配置数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<TypeModuleArgumentModel> FindAsync(Expression<Func<TypeModuleArgumentModel, bool>> predicate)
        {
            var data = await _db.TypeModuleArguments.AsNoTracking().Where(predicate).FirstOrDefaultAsync();
            return data;
        }

        /// <summary>
        /// 获取模块下所有的配置数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<TypeModuleArgumentModel>> FindWithTypeDataDefineAsync(Expression<Func<TypeModuleArgumentModel, bool>> predicate)
        {
            var data = await _db.TypeModuleArguments.AsNoTracking().Include(a => a.TypeDataDefine).Where(predicate).ToListAsync();
            return data;
        }
    }
}
