using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeModuleControlService : IBaseService<TypeModuleControlModel>
    {
        /// <summary>
        /// 添加模块控制项
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="ModuleId">模块标示</param>
        /// <param name="req">控制项数据</param>
        /// <returns></returns>
        Task<BaseResponse> AddTypeModuleControlAsync(string Account,/* int ModuleId,*/ TypeModuleControlAddDto req);
        /// <summary>
        /// 编辑模块控制项
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">模块控制项数据</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateTypeModuleControlAsync(string Account,  TypeModuleControlUpdateDto req);
        /// <summary>
        /// 删除模块控制项数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="Id">控制项标示</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteTypeModuleControlAsync(string Account, int Id);
        /// <summary>
        /// 根据模块标示获取控制项数据
        /// </summary>
        /// <param name="ModuleId">模块标示</param>
        /// <returns></returns>
        Task<BaseResponse> GetTypeModuleControlsByModuleIdAsync(int ModuleId);
        /// <summary>
        /// 根据标示获取模块控制项数据
        /// </summary>
        /// <param name="Id">控制项标示</param>
        /// <returns></returns>
        Task<BaseResponse> GetTypeModuleControlsByIdAsync(int Id);
    }
}
