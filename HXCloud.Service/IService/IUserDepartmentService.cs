using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IUserDepartmentService : IBaseService<UserDepartmentModel>
    {
        HandleResponse<UserDepartmentKey> AddUserDepartment(UserDepartmentAddViewModel req);
        HandleResponse<UserDepartmentKey> RemoveUserDepartment(int userId, int departmentId);
        UserDepartmentListViewModel GetUserDepartment(int Id);
    }
}
