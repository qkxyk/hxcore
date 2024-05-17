using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeConfigService : IBaseService<TypeConfigModel>
    {
        Task<BaseResponse> AddAsync(int typeId, TypeConfigAddViewModel req, string account);
        Task<BaseResponse> UpdateAsync(TypeConfigUpdateViewModel req, string account);
        Task<BaseResponse> DeleteAsync(int Id, string account);
        Task<BaseResponse> FindById(int Id);
        Task<BaseResponse> FindByType(int typeId, TypeConfigPageRequestViewModel req);
        bool IsExist(Expression<Func<TypeConfigModel, bool>> predicate, out string GroupId);
        /// <summary>
        /// 用于httppath部分修改数据
        /// </summary>
        /// <param name="Id">类型配置数据标识</param>
        /// <returns></returns>
        Task<TypeConfigData> GetTypeConfigByIdAsync(int Id);
        /// <summary>
        /// 修改类型配置数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">修改数据</param>
        /// <returns></returns>
        Task<BaseResponse> PatchTypeConfigAsync(string Account, TypeConfigData req);
    }
}
