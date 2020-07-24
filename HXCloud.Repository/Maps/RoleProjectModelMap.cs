using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HXCloud.Repository.Maps
{
    public class RoleProjectModelMap : BaseModelMap<RoleProjectModel>
    {
        public override void Configure(EntityTypeBuilder<RoleProjectModel> builder)
        {
            builder.ToTable("RoleProject").HasKey(a => new { a.RoleId, a.ProjectId });
            builder.HasOne(a => a.Role).WithMany(a => a.RoleProjects).HasForeignKey(a => a.RoleId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(a => a.Project).WithMany(a => a.RoleProjects).HasForeignKey(a => a.ProjectId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
