using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDepartmentService : IBaseService<DepartmentModel>
    {
        Task<BaseResponse> AddDepartmentAsync(DepartmentAddViewModel req, string account);
        Task<BaseResponse> UpdateDepartmentAsync(DepartmentUpdateViewModel req, string groupId, string account);
        Task<BaseResponse> DeleteDepartmentAsync(int id, string account);
        //bool IsExist(Expression<Func<DepartmentModel, bool>> predicate);
        Task<BaseResponse> GetDepartment(int id);
        Task<string> GetDepartmentGroupAsync(int Id);
        Task<BaseResponse> GetGroupDepartment(string GroupId);
    }
}
