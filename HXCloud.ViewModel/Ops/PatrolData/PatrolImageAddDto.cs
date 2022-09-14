using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class PatrolImageAddDto
    {
        /// <summary>
        /// 问题图片，支持多张图片
        /// </summary>
        public string Url { get; set; }
        public string PatrolId { get; set; }
    }
}
