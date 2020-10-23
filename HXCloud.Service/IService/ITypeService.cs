using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeService : IBaseService<TypeModel>
    {
        Task<BaseResponse> AddType(TypeAddViewModel req, string account);
        Task<BaseResponse> UpdateType(TypeUpdateViewModel req, string account);
        Task<string> GetTypeGroupIdAsync(int Id);
        Task<BaseResponse> DeleteTypeAsync(int Id, string account);
        Task<BaseResponse> GetGroupTypeAsync(string GroupId);
        Task<BaseResponse> GetTypeAsync(int Id);
        /// <summary>
        /// 验证该类型下是否能添加相关的信息
        /// </summary>
        /// <param name="Id">类型标示</param>
        /// <param name="GroupId">类型所属的组织编号</param>
        /// <param name="status">类型是否能是叶子节点</param>
        /// <returns>类型是否存在</returns>
        Task<TypeCheckDto> CheckTypeAsync(int Id);
        /// <summary>
        /// 验证该类型下是否能添加相关的信息
        /// </summary>
        /// <param name="Id">类型标示</param>
        /// <param name="GroupId">类型所属的组织编号</param>
        /// <param name="status">类型是否能是叶子节点</param>
        /// <returns>类型是否存在</returns>
        Task<TypeCheckDto> CheckTypeAsync(Expression<Func<TypeModel, bool>> prediceate);
        bool IsExist(int Id, out string GroupId);
        bool IsExist(Expression<Func<TypeModel, bool>> predicate, out string GroupId);
        Task<BaseResponse> CopyTypeAsync(string Account, int sourceId, int targetId);
        /// <summary>
        /// 类型文件拷贝
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="FilePath">类型文件路径,路径到Typefiles</param>
        /// <param name="ImagePath">类型图片路径，路径到TypeImage</param>
        /// <param name="SourceId">源类型</param>
        /// <param name="TargetId">目标类型</param>
        /// <returns>是否拷贝成功</returns>
        Task<BaseResponse> CopyTypeFilesAsync(string Account, string FilePath, string ImagePath, int SourceId, int TargetId);
    }
}
