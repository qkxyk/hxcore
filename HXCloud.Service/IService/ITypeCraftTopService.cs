using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface ITypeCraftTopService : IBaseService<TypeCraftTopModel>
    {
        /// <summary>
        /// 查询拓扑数据是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TypeCraftTopExistDto> IsCraftTopExist(Expression<Func<TypeCraftTopModel, bool>> predicate);
        /// <summary>
        /// 添加类型拓扑数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">网络拓扑数据</param>
        /// <returns></returns>
        Task<BaseResponse> AddTypeCraftTopAsync(string account, TypeCraftTopAddDto req);
        /// <summary>
        /// 根据拓扑数据标识获取拓扑数据
        /// </summary>
        /// <param name="Id">拓扑数据标识</param>
        /// <returns></returns>
        Task<BaseResponse> GetTypeCraftTopByIdAsync(int Id);

        /// <summary>
        /// 获取类型拓扑数据
        /// </summary>
        /// <param name="typeId">类型标识</param>
        /// <returns></returns>
        Task<BaseResponse> GetTypeCraftTopAsync(int typeId);
        /// <summary>
        /// 删除类型拓扑数据
        /// </summary>
        /// <param name="id">数据标识</param>
        /// <param name="account">操作人</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <param name="path">文件的路径</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteTypeCraftTopAsync(int id, string account, bool isAdmin, string path);
        /// <summary>
        /// 修改类型拓扑数据
        /// </summary>
        /// <param name="account">修改人</param>
        /// <param name="req">拓扑数据信息</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateTypeCraftTopAsync(string account, TypeCraftTopEditDto req);
    }
}
