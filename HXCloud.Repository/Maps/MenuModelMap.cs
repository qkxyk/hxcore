using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class MenuModelMap : IEntityTypeConfiguration<MenuModel>
    {
        public void Configure(EntityTypeBuilder<MenuModel> builder)
        {
            builder.ToTable("Menu").HasKey(a => a.Id);
            builder.HasOne(a => a.Group).WithMany(a => a.Menus).HasForeignKey(a => a.GroupId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.Parent).WithMany(a => a.Child).HasForeignKey(a => a.ParentId).IsRequired(false);//.OnDelete( DeleteBehavior.ClientSetNull);
        }
    }
}
