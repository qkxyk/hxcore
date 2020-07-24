using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class GroupListViewModel : BaseResponse
    {
        public GroupListViewModel()
        {
            Data = new List<GroupData>();
            //Data = new Test();
        }
        public List<GroupData> Data { get; set; }
        //public Test Data { get; set; }
    }
    public class Test
    {
        public Test()
        {
            list = new List<GroupData>();
        }
        public List<GroupData> list { get; set; }
    }
    public class TestListViewModel : BResponse<Test>
    {
        public TestListViewModel()
        {
            Data = new Test();
        }
    }


}
