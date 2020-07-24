using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class ProjectImageModel : BaseModel,IAggregateRoot
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string url { get; set; }
        public int Rank { get; set; } = 1;//图片顺序
        public int ProjectId { get; set; }
        public virtual ProjectModel project { get; set; }
    }
}
