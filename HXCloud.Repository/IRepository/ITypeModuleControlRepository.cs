using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeModuleControlRepository : IBaseRepository<TypeModuleControlModel>
    {
        /// <summary>
        /// 获取控制项和控制项下的反馈数据，主要用来检查控制项下是否存在反馈项
        /// </summary>
        /// <param name="Id">控制项标示</param>
        /// <returns></returns>
        Task<TypeModuleControlModel> FindWithFeedbackAsync(int Id);
        /// <summary>
        /// 获取控制项和控制项下反馈数据，以及关联的数据定义
        /// </summary>
        /// <param name="predicate">控制项条件</param>
        /// <returns></returns>
        Task<IEnumerable<TypeModuleControlModel>> FindWithFeedbackAndDataDefineAsync(Expression<Func<TypeModuleControlModel, bool>> predicate);
    }
}
