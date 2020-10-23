using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeRepository : IBaseRepository<TypeModel>
    {
        Task<TypeModel> FindAsync(Expression<Func<TypeModel, bool>> predicate);
        /// <summary>
        /// 类型拷贝(类型拷贝分两步，第一步拷贝类型，第二步拷贝类型的更新文件和工艺图)
        /// </summary>
        /// <param name="sourceId">源类型标示</param>
        /// <param name="target">目标类型</param>
        /// <returns></returns>
        Task CopyTypeAsync(int sourceId, TypeModel target);
        /// <summary>
        /// 拷贝类型文件，包含类型更新文件，类型工艺图
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="filePath">文件路径(绝对路径,只到末端的文件夹)</param>
        ///<param name="imagePath">工艺图路径(绝对路径，只到末端的文件夹)</param>
        /// <param name="sourceId">源类型标示</param>
        /// <param name="targetId">目标类型标示</param>
        /// <returns></returns>
        Task CopyTypeFileAsync(string Account, string filePath, string imagePath, int sourceId, int targetId);
    }
}
