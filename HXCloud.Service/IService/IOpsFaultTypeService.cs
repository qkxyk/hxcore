using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IOpsFaultTypeService : IBaseService<OpsFaultTypeModel>
    {
        /// <summary>
        /// 验证运维故障类型数据是否存在，不存在返回-1,存在则返回该类型子节点或者顶级节点
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> IsExistAsync(Expression<Func<OpsFaultTypeModel, bool>> predicate);
        /// <summary>
        /// 添加故障类型,故障类型只能添加两层
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">故障类型数据</param>
        /// <returns></returns>
        /// <summary>
        /// 修改故障类型数据，只能修改故障类型名称
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<BaseResponse> EditOpsFaultTypeAsync(string account, OpsFaultTypeEditDto req);
        /// <summary>
        /// 删除故障类型
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">故障类型编号</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteOpsFaultTypeAsync(string account, int Id);
        Task<BaseResponse> AddOpsFaultTypeAsync(string account, OpsFaultTypeAddDto req);
        /// <summary>
        /// 根据故障类型Id获取故障类型，包含该故障类型关联的所有故障数据，包含验证故障类型是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Flag">是否顶级节点</param>
        /// <returns></returns>
        Task<BaseResponse> GetOpsFaultTypeByIdAsync(int Id, int Flag = 0);

        /// <summary>
        /// 获取全部故障类型数据
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse> GetAllOpsFaultTypeAsync();
        /// <summary>
        /// 根据父标识获取全部故障类型
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<BaseResponse> GetOpsFaultTypeByParentIdAsync(int Id);
    }
}
