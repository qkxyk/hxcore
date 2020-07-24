using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeModelMap : BaseModelMap<TypeModel>
    {
        public override void Configure(EntityTypeBuilder<TypeModel> builder)
        {
            builder.ToTable("Type").HasKey(a => a.Id);
            builder.HasOne(a => a.Group).WithMany(a => a.Types).HasForeignKey(a => a.GroupId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.Parent).WithMany(a => a.Child).HasForeignKey(a => a.ParentId).IsRequired(false);
            base.Configure(builder);
        }
    }
}
