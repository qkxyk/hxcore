using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 系统模块，设置模块是为了根据模块分配模块的权限。例如运维模块，
    /// </summary>
    public class ModuleModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }//模块标识
        public string ModuleName { get; set; }//模块名称
        public string Code { get; set; }//模块的编号，例如ops代表运维模块，deviceMonitor代表设备监控模块,系统默认是设备监控模块
        public virtual ICollection<ModuleOperateModel> Operates { get; set; }//模块的操作
        public virtual ICollection<RoleModel> Roles { get; set; }//模块的角色
    }
    public class ModuleOperateModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }//操作标识
        public string OperateName { get; set; }//操作名称
        public int ModuleId { get; set; }//所属模块标识
        public string Code { get; set; }//操作码，用于权限验证
        public int SerialNumber { get; set; }//编号
        public string SerialName { get; set; }//编号名称,如设备巡检、问题上报、设备运维
        public virtual ModuleModel Module { get; set; }//关联的模块
        public ICollection<RoleModuleOperateModel> RoleModuleOperates { get; set; }//角色权限
    }

    /// <summary>
    /// 角色模块操作
    /// </summary>
    public class RoleModuleOperateModel:BaseCModel
    {
        public int RoleId { get; set; }//角色标识
        public int OperateId { get; set; }//模块操作标识
        public RoleModel Role { get; set; }//关联的角色
        public ModuleOperateModel ModuleOperate { get; set; }//关联的模块操作
    }
}
