using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
   public class TypeAccessoryModelMap:BaseModelMap<TypeAccessoryModel>
    {
        public override void Configure(EntityTypeBuilder<TypeAccessoryModel> builder)
        {
            builder.ToTable("TypeAccessory").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeAccessories).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
