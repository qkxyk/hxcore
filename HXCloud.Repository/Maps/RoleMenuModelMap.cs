using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class RoleMenuModelMap : IEntityTypeConfiguration<RoleMenuModel>
    {
        public void Configure(EntityTypeBuilder<RoleMenuModel> builder)
        {
            builder.ToTable("RoleMenu").HasKey(a => new { a.RoleId, a.MenuId });
            builder.HasOne(a => a.Role).WithMany(a => a.RoleMenus).HasForeignKey(a => a.RoleId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(a => a.Menu).WithMany(a => a.RoleMenus).HasForeignKey(a => a.MenuId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
