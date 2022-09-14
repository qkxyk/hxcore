using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairAddImageDto
    {
        public string Description { get; set; }
        /// <summary>
        /// 问题图片，支持多张图片
        /// </summary>
        public string Url { get; set; }
        public string Id { get; set; }
    }
}
