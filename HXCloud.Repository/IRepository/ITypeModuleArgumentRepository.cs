using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public interface ITypeModuleArgumentRepository : IBaseRepository<TypeModuleArgumentModel>
    {
        /// <summary>
        /// 查找模块配置数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TypeModuleArgumentModel> FindAsync(Expression<Func<TypeModuleArgumentModel, bool>> predicate);
        /// <summary>
        /// 获取模块下所有的配置数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<TypeModuleArgumentModel>> FindWithTypeDataDefineAsync(Expression<Func<TypeModuleArgumentModel, bool>> predicate);
    }
}
