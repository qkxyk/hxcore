using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeConfigModelMap : BaseModelMap<TypeConfigModel>
    {
        public override void Configure(EntityTypeBuilder<TypeConfigModel> builder)
        {
            builder.ToTable("TypeConfig").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeConfig).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
