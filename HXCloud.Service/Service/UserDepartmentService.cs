using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public class UserDepartmentService : IUserDepartmentService
    {
        private IUserDepartmentRepository _UserDepartment;

        public UserDepartmentService(IUserDepartmentRepository userDepartment)
        {
            _UserDepartment = userDepartment;
        }
        public HandleResponse<UserDepartmentKey> AddUserDepartment(UserDepartmentAddViewModel req)
        {
            HandleResponse<UserDepartmentKey> rm = new HandleResponse<UserDepartmentKey>();
            //bool b =await IsExist(a => a.UserId == req.UserId && a.DeparmentId == req.DepartmentId);
            //if (b)
            //{
            //    rm.Success = false;
            //    rm.Message = "该用户已分配在该部门，请勿重复操作";
            //    return rm;
            //}
            UserDepartmentModel ud = new UserDepartmentModel() { UserId = req.UserId, DeparmentId = req.DepartmentId };
            try
            {
                _UserDepartment.Add(ud);
                rm.Success = true;
                rm.Message = "添加数据成功";
                rm.Key = new UserDepartmentKey { UserId = ud.UserId, DepartmentId = ud.DeparmentId };
            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Message = "添加数据失败" + ex.Message;
            }
            return rm;
        }
        public HandleResponse<UserDepartmentKey> RemoveUserDepartment(int userId, int departmentId)
        {
            HandleResponse<UserDepartmentKey> rm = new HandleResponse<UserDepartmentKey>();

            UserDepartmentModel ud = new UserDepartmentModel() { UserId = userId, DeparmentId = departmentId };
            try
            {
                _UserDepartment.Remove(ud);
                rm.Success = true;
                rm.Message = "删除数据成功";
                rm.Key = new UserDepartmentKey { UserId = ud.UserId, DepartmentId = ud.DeparmentId };
            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Message = "删除数据失败" + ex.Message;
            }
            return rm;
        }

        public UserDepartmentListViewModel GetUserDepartment(int Id)
        {
            UserDepartmentListViewModel rm = new UserDepartmentListViewModel();
            //bool b =await IsExist(a => a.UserId == Id);
            //if (!b)
            //{
            //    rm.Success = true;
            //    rm.Message = "该用户没有分配任何部门";
            //    return rm;
            //}
            var ud = _UserDepartment.FindWithDepartment(a => a.UserId == Id);
            foreach (var item in ud)
            {
                rm.Data.Add(new UserDepartmentData
                {
                    UserId = item.UserId,
                    DepartmentId = item.DeparmentId,
                    DepartmentName = item.Department.DepartmentName,
                    DepartmentPathName = item.Department.PathName
                });
            }
            rm.Success = true;
            rm.Message = "获取数据成功";
            return rm;
        }

        public async Task<bool> IsExist(Expression<Func<UserDepartmentModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
