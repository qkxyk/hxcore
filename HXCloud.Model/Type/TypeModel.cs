using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class TypeModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }//设备类型标识
        public string TypeName { get; set; }//设备类型名称
        public Nullable<int> ParentId { get; set; } //设备所属的父类型标识
        public string Description { get; set; }//设备类型描述
        public virtual TypeModel Parent { get; set; }//设备类型关联的父设备类型
        public virtual ICollection<TypeModel> Child { get; set; }//设备类型关联的子设备类型
        public string PathId { get; set; }
        public string PathName { get; set; }
        public string ICON { get; set; }//类型图标名称（后台只存图标名称,图标放在客户端）2019-1-10添加
        public TypeStatus Status { get; set; }
        public string GroupId { get; set; }//组织编号
        public virtual GroupModel Group { get; set; }

        public virtual ICollection<TypeImageModel> TypeImages { get; set; }//设备的工艺图
        public virtual ICollection<TypeUpdateFileModel> TypeUpdateFiles { get; set; }//设备更新文件
        public virtual ICollection<TypeStatisticsInfoModel> TypeStatisticsInfo { get; set; }  //设备统计数据
        public virtual ICollection<TypeDataDefineModel> TypeDataDefine { get; set; }  //设备书定义

        public virtual ICollection<TypeConfigModel> TypeConfig { get; set; }//类型配置
        public virtual ICollection<TypeArgumentModel> TypeArguments { get; set; }   //类型参数，该类型下所有的设备数据都相同

        public virtual ICollection<TypeSchemaModel> Schemas { get; set; }//设备类型的模式
        public virtual ICollection<DeviceModel> Devices { get; set; }
        public virtual ICollection<TypeHardwareConfigModel> TypeHardwareConfig { get; set; }
        public virtual ICollection<TypeAccessoryModel> TypeAccessories { get; set; }
        public virtual ICollection<TypeSystemModel> TypeSystems { get; set; }

        public virtual ICollection<TypeOverviewModel> TypeOverviews { get; set; }
        public virtual ICollection<TypeDisplayIconModel> TypeDisplayIcons { get; set; }
        public virtual ICollection<TypeModuleModel> TypeModules { get; set; }
    }
    public enum TypeStatus
    {
        Root, Leaf
    }
}
