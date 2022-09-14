using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class TypeOpsItemModelMap : BaseModelMap<TypeOpsItemModel>
    {
        public override void Configure(EntityTypeBuilder<TypeOpsItemModel> builder)
        {
            builder.ToTable("TypeOpsItem").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeOpsItems).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
