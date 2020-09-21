using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    /// <summary>
    /// 主要用来配置类型关联的设备列表中显示设备运行状态图标
    /// </summary>
    public interface ITypeDisplayIconService : IBaseService<TypeDisplayIconModel>
    {
        /// <summary>
        /// 添加类型显示图标，图标保存在客户端，服务端只保存图标名称
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="TypeId">类型标示</param>
        /// <param name="req">类型显示图标数据</param>
        /// <returns></returns>
        Task<BaseResponse> AddTypeDisplayIconAsync(string Account, int TypeId, TypeDisplayIconAddDto req);
        /// <summary>
        /// 修改类型显示图标类型
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="TypeId">类型标示</param>
        /// <param name="req">类型显示图标数据</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateTypeDisplayIconAsync(string Account, int TypeId, TypeDisplayIconUpdateDto req);
        /// <summary>
        /// 删除类型显示图标数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="Id">类型显示图标标示</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteDisplayIconAsync(string Account, int Id);
        /// <summary>
        /// 获取类型显示图标，获取全部
        /// </summary>
        /// <param name="TypeId">类型标示</param>
        /// <returns></returns>
        Task<BaseResponse> GetTypeDisplayByTypeIdAsync(int TypeId);
    }
}
