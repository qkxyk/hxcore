using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public interface IOpsFaultTypeRepository : IBaseRepository<OpsFaultTypeModel>
    {
        /// <summary>
        /// 顶级节点
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<OpsFaultTypeModel> FindWithOpsFaultAsync(int Id);
        /// <summary>
        /// 叶子节点
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<OpsFaultTypeModel> FindWithOpsFaultChildAsync(int Id);
        Task<List<OpsFaultTypeModel>> FindAllWithOpsFaultAsync();
        Task<List<OpsFaultTypeModel>> FindByParentIdWithOpsFaultAsync(int Id);
    }
}
