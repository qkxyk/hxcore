using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeSchemaModelMap : BaseModelMap<TypeSchemaModel>
    {
        public override void Configure(EntityTypeBuilder<TypeSchemaModel> builder)
        {
            builder.ToTable("TypeSchema").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.Schemas).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.Parent).WithMany(a => a.Child).HasForeignKey(a => a.ParentId).IsRequired(false);     
            base.Configure(builder);
        }
    }
}
