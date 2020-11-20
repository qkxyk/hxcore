using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeUpdateFileData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Version { get; set; }
        #region 2020-11-9 新增更新文件层级,默认为0表示主系统，1表示子系统，2表示bootloader
        public int Level { get; set; } = 0;
        #endregion
        public string Description { get; set; }

        public int TypeId { get; set; }
    }
}
