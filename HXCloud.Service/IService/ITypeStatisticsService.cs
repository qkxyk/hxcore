using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeStatisticsService : IBaseService<TypeStatisticsInfoModel>
    {
        bool IsExist(Expression<Func<TypeStatisticsInfoModel, bool>> predicate, out string GroupId);
        Task<BaseResponse> AddStatistics(int typeId, TypeStatisticsAddViewModel req, string account);
        Task<BaseResponse> UpdateStatistics(TypeStatisticsUpdateViewModel req, string account);
        Task<BaseResponse> DeleteAsync(int Id, string account);
        Task<BaseResponse> GetStatisticsAsync(int Id);
        Task<BaseResponse> FindByTypeAsync(int typeId, TypeStatisticsPageRequestViewModel req);
    }
}
