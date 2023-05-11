using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IModuleOperateService:IBaseService<ModuleOperateModel>
    {
        /// <summary>
        /// 添加模块操作
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">模块操作数据</param>
        /// <returns></returns>
        Task<BaseResponse> AddModuleOperateAsync(string Account, ModuleOperateAddDto req);
              /// <summary>
              /// 根据模块操作标识获取模块操作数据
              /// </summary>
              /// <param name="Id">模块操作标识</param>
              /// <returns></returns>
        Task<BaseResponse> GetModuleOperateByIdAsync(int Id);
              /// <summary>
              /// 获取模块操作
              /// </summary>
              /// <param name="ModuleId">模块标识</param>
              /// <returns></returns>
        Task<BaseResponse> GetModuleOperatesAsync(int ModuleId);
          /// <summary>
          /// 删除模块操作数据
          /// </summary>
          /// <param name="Account">操作人</param>
          /// <param name="Id">模块操作标识</param>
          /// <returns></returns>
        Task<BaseResponse> DeleteModuleOperateByIdAsync(string Account, int Id);
    }
}
