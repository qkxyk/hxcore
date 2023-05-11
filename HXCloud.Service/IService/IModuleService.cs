using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IModuleService : IBaseService<ModuleModel>
    {
        /// <summary>
        /// 用于httppath部分修改数据
        /// </summary>
        /// <param name="Id">模块标识</param>
        /// <returns></returns>
        public Task<ModuleDto> GetModuleByIdAsync(int Id);
        public Task<BaseResponse> GetModuleDataByIdAsync(int Id);
        Task<BaseResponse> GetModulesAsync();
        /// <summary>
        /// 添加系统模块
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">模块参数</param>
        /// <returns></returns>
        Task<BaseResponse> AddModuleAsync(string Account, ModuleAddDto req);
        Task<BaseResponse> UpdateModuleAsync(string Account, int Id, ModuleAddDto req);
        Task<BaseResponse> DeleteModuleAsync(string Account, int Id);
        /// <summary>
        /// 修改模块数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">修改数据</param>
        /// <returns></returns>
        Task<BaseResponse> PatchModuleAsync(string Account, ModuleDto req);
    }
}
