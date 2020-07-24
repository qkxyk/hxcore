using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class UserRoleModelMap : BaseModelMap<UserRoleModel>
    {
        public override void Configure(EntityTypeBuilder<UserRoleModel> builder)
        {
            builder.ToTable("UserRole").HasKey(a => new { a.UserId, a.RoleId });
            builder.HasOne(a => a.User).WithMany(a => a.UserRoles).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(a => a.Role).WithMany(a => a.UserRoles).HasForeignKey(a => a.RoleId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
