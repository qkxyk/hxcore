using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeModuleFeedbackRepository : IBaseRepository<TypeModuleFeedbackModel>
    {
        /// <summary>
        /// 获取反馈数据以及关联的类型数据定义
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<TypeModuleFeedbackModel>> FindWithDataDefineAsync(Expression<Func<TypeModuleFeedbackModel, bool>> predicate);
    }
}
