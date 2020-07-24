using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeImageModelMap : BaseModelMap<TypeImageModel>
    {
        public override void Configure(EntityTypeBuilder<TypeImageModel> builder)
        {
            builder.ToTable("TypeImage").HasKey(a => a.Id);
            builder.Property(a => a.Rank).HasDefaultValue(1);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeImages).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
