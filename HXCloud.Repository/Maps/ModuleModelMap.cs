using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class ModuleModelMap:BaseModelMap<ModuleModel>
    {
        public override void Configure(EntityTypeBuilder<ModuleModel> builder)
        {
            builder.ToTable("Module").HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
    public class ModuleOperateModelMap : BaseModelMap<ModuleOperateModel>
    {
        public override void Configure(EntityTypeBuilder<ModuleOperateModel> builder)
        {
            builder.ToTable("ModuleOperate").HasKey(a => a.Id);
            builder.HasOne(a => a.Module).WithMany(a => a.Operates).HasForeignKey(a => a.ModuleId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }

    public class RoleModuleOperateModelMap : BaseModelMap<RoleModuleOperateModel>
    {
        public override void Configure(EntityTypeBuilder<RoleModuleOperateModel> builder)
        {
            builder.ToTable("RoleModuleOperate").HasKey(a => new { a.RoleId, a.OperateId });
            builder.HasOne(a => a.Role).WithMany(a => a.RoleModuleOperates).HasForeignKey(a => a.RoleId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.ModuleOperate).WithMany(a => a.RoleModuleOperates).HasForeignKey(a => a.OperateId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
