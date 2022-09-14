using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 更改用户的运维信息
    /// </summary>
    public class UserOpsUpdateDto
    {
        [Required(ErrorMessage ="要修改的用户标识不能为空")]
        public int Id { get; set; }//用户标示
        [Range(0, 3, ErrorMessage = "运维人员分类只能输入0，3")]
        /// <summary>
        /// 用户分类，用来区分是巡检和维修人员,0为一般人员，2，3为运维人员  巡检、维修人员和负责运维的经理
        /// </summary>
        public int Category { get; set; } = 0;
        /// <summary>
        /// 用户是否有上级，只要用来获取运维人员
        /// </summary>
        public int? ParentId { get; set; }
    }
}
