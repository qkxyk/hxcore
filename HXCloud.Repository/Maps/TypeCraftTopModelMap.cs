using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class TypeCraftTopModelMap : BaseModelMap<TypeCraftTopModel>
    {
        public override void Configure(EntityTypeBuilder<TypeCraftTopModel> builder)
        {
            builder.ToTable("TypeCraftTop").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeCraftTops).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
