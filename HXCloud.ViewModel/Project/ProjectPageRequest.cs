using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class ProjectPageRequest : BasePageRequest
    {
        [Range(0,2,ErrorMessage ="项目类型只能在0到2之间")]
        public int ProjectType { get; set; } = 0;//默认为全部
    }
}
