using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeModuleRepository : IBaseRepository<TypeModuleModel>
    {
        /// <summary>
        /// 获取模块和模块下的控制项，主要用来检查模块下是否存在控制项
        /// </summary>
        /// <param name="Id">模块标示</param>
        /// <returns></returns>
        Task<TypeModuleModel> FindWithControlAsync(int Id);
        /// <summary>
        /// 获取模块数据，以及模块下的控制项和反馈数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<IEnumerable<TypeModuleModel>> FindWithControlAndFeedbackAsync(Expression<Func<TypeModuleModel, bool>> predicate);
    }
}
