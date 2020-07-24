using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class RoleModelMap : BaseModelMap<RoleModel>
    {
        public override void Configure(EntityTypeBuilder<RoleModel> builder)
        {
            builder.ToTable("Role").HasKey(a => a.Id);
            builder.Property(a => a.IsAdmin).HasDefaultValue(false);
            builder.HasOne(a => a.Group).WithMany(a => a.Roles).HasForeignKey(a => a.GroupId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
            //builder.HasOne(a => a.Department).WithMany(a => a.Roles).HasForeignKey(a => a.DepartmentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
