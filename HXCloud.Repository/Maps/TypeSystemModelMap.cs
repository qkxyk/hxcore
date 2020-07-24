using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeSystemModelMap : BaseModelMap<TypeSystemModel>
    {
        public override void Configure(EntityTypeBuilder<TypeSystemModel> builder)
        {
            builder.ToTable("TypeSystem").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeSystems).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
