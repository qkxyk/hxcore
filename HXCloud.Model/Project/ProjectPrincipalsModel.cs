using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //项目负责人，只添加到顶级项目中，用来处理项目中的一些运维信息
    public class ProjectPrincipalsModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int ProjectId { get; set; }
        public  ProjectModel Project { get; set; }
    }
}
