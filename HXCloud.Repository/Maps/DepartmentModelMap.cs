using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DepartmentModelMap : BaseModelMap<DepartmentModel>
    {
        public override void Configure(EntityTypeBuilder<DepartmentModel> builder)
        {
            builder.ToTable("Department").HasKey(a => a.Id);
            builder.Property(a => a.Level).HasDefaultValue(0);
            builder.HasOne(a => a.Group).WithMany(a => a.Departments).HasForeignKey(a => a.GroupId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.Parent).WithMany(a => a.Child).HasForeignKey(a => a.ParentId);//.IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);
            base.Configure(builder);
        }
    }
}
