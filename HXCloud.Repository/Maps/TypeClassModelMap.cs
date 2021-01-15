using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class TypeClassModelMap : BaseModelMap<TypeClassModel>
    {
        public override void Configure(EntityTypeBuilder<TypeClassModel> builder)
        {
            builder.ToTable("TypeClass").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeClasses).HasForeignKey(f => f.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
