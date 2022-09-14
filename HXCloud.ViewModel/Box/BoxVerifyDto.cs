using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class BoxVerifyDto
    {
        //返回加密数据
        public string Data { get; set; }
    }

    public class BoxVerifyReqiredDto
    {
        [Required(ErrorMessage ="uuid不能为空")]
        public string UUID { get; set; }
        [Required(ErrorMessage ="serial不能为空")]
        public string Serial { get; set; }
        [Required(ErrorMessage ="imei不能为空")]
        public string Imei { get; set; }
    }
}
