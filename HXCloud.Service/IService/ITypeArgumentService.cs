using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeArgumentService : IBaseService<TypeArgumentModel>
    {
        bool IsExist(Expression<Func<TypeArgumentModel, bool>> predicate, out string GroupId);
        Task<BaseResponse> AddArgumentAsync(int typeId, TypeArgumentAddViewModel req, string account);
        Task<BaseResponse> UpdateTypeArgumentAsync(int typeId, TypeArgumentUpdateViewModel req, string account);
        Task<BaseResponse> GetArgumentAsync(int Id);
        Task<BaseResponse> GetTypeArgumentAsync(int typeId, BasePageRequest req);
        Task<BaseResponse> DeleteTypeArgumentAsync(int Id, string account);
        /// <summary>
        /// 根据类别查找类型配置参数
        /// </summary>
        /// <param name="TypeId">类型编号</param>
        /// <param name="Category">类别名称</param>
        /// <returns></returns>
        Task<BaseResponse> GetTypeArgumentByCategory(int TypeId, string Category);
    }
}
