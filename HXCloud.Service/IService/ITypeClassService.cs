using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface ITypeClassService : IBaseService<TypeClassModel>
    {
        /// <summary>
        /// 添加类型分组数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="TypeId">类型编号</param>
        /// <param name="req">类型分组数据</param>
        /// <returns></returns>
        Task<BaseResponse> AddTypeClassAsync(string Account, int TypeId, TypeClassAddDto req);
        /// <summary>
        /// 删除类型分组数据
        /// </summary>
        /// <param name="Id">类型分组标识</param>
        /// <param name="account">操作人</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteAsync(int Id, string account);
        /// <summary>
        /// 获取类型分组信息
        /// </summary>
        /// <param name="TypeId">类型标识</param>
        /// <returns></returns>
        Task<BaseResponse> GetByTypeIdAsync(int TypeId);

        /// <summary>
        /// 更新类型分组数据
        /// </summary>
        /// <param name="req">类型分组信息</param>
        /// <param name="account">操作人</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateAsync(TypeClassUpdateDto req, string account);
    }
}
