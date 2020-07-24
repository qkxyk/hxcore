using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeArgumentModelMap : BaseModelMap<TypeArgumentModel>
    {
        public override void Configure(EntityTypeBuilder<TypeArgumentModel> builder)
        {
            builder.ToTable("TypeArgument").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeArguments).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
