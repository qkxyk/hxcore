using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class UserDepartmentData
    {
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentPathName { get; set; }
    }

    public class UserDepartmentListViewModel : BaseResponse
    {
        public UserDepartmentListViewModel()
        {
            Data = new List<UserDepartmentData>();
        }
        public List<UserDepartmentData> Data { get; set; }
    }
}
