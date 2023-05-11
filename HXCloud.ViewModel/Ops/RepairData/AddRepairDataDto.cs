using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class AddRepairDataDto : AddRepairDataMessageDto
    {
        public string Url { get; set; }//关联文件的地址
    }
}
