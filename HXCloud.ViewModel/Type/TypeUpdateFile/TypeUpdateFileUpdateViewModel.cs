using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeUpdateFileUpdateViewModel
    {
        [Required(ErrorMessage ="升级文件的标示必须输入")]
        public int Id { get; set; }
        [Required(ErrorMessage ="升级文件的名称必须输入")]
        public string Name { get; set; }
        [Required(ErrorMessage ="升级文件的版本号必须输入")]
        public string Version { get; set; }
        [Required(ErrorMessage ="类型编号必须输入")]
        public int TypeId { get; set; }
        #region 2020-11-9 新增更新文件层级,默认为0表示主系统，1表示子系统，2表示bootloader
        [Range(0, 2, ErrorMessage = "文件层级只能输入0到2")]
        public int Level { get; set; } = 0;
        #endregion
        public string Description { get; set; }

    }
}
