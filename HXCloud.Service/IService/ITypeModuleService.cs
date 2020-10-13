using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeModuleService : IBaseService<TypeModuleModel>
    {
        /// <summary>
        /// 添加类型模块
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="TypeId">类型标示</param>
        /// <param name="req">模块数据</param>
        /// <returns></returns>
        Task<BaseResponse> AddTypeModuleAsync(string Account, int TypeId, TypeModuleAddDto req);
        /// <summary>
        /// 修改类型模块
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="TypeId">类型标示</param>
        /// <param name="req">模块数据</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateTypeModuleAsync(string Account, int TypeId, TypeModuleUpdateDto req);
        /// <summary>
        /// 删除类型模块
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="Id">类型模块标示</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteTypeModuleAsync(string Account, int Id);
        /// <summary>
        /// 获取类型全部模块数据
        /// </summary>
        /// <param name="TypeId">类型标示</param>
        /// <returns></returns>
        Task<BaseResponse> GetTypeModulesByTypeIdAsync(int TypeId);
        /// <summary>
        /// 根据标示获取模块数据
        /// </summary>
        /// <param name="Id">模块标示</param>
        /// <returns></returns>
        Task<BaseResponse> GetTypeModuleByIdAsync(int Id);
    }
}
