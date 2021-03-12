using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    //验证模块是否存在，故传入模块类型
    public interface ITypeModuleArgumentService : IService<TypeModuleModel, TypeModuleArgumentCheckDto>
    {
        /// <summary>
        /// 添加模块配置项数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="ModuleId">模块标识</param>
        /// <param name="req">配置项数据</param>
        /// <returns>返回操作数据</returns>
        Task<BaseResponse> AddTypeModuleArgumentAsync(string Account, int ModuleId, int TypeId, TypeModuleArgumentAddDto req);
        /// <summary>
        /// 根据模块标识获取模块配置数据
        /// </summary>
        /// <param name="ModuleId">模块标识</param>
        /// <returns>获取模块下的配置数据</returns>
        Task<BaseResponse> GetTypeModuleArgumentAsync(int ModuleId);

        /// <summary>
        /// 删除模块配置数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="Id">模块配置数据标识</param>
        /// <returns>返回删除数据是否成功信息</returns>
        Task<BaseResponse> DeleteTypeModuleArgumentAsync(string Account, int Id);
        /// <summary>
        /// 更改模块的配置数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="ModuleId">模块标识</param>
        /// <param name="req">模块配置数据</param>
        /// <returns>返回修改模块配置数据是否成功</returns>
        Task<BaseResponse> UpdateModuleArgumentAsync(string Account, int ModuleId, int TypeId, TypeModuleArgumentUpdateDto req);
    }
}
