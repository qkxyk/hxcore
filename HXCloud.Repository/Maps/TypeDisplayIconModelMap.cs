using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeDisplayIconModelMap:BaseModelMap<TypeDisplayIconModel>
    {
        public override void Configure(EntityTypeBuilder<TypeDisplayIconModel> builder)
        {
            builder.ToTable("TypeDisplayIcon").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeDisplayIcons).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.TypeDataDefine).WithMany(a => a.TypeDisplayIcons).HasForeignKey(a => a.DataDefineId).OnDelete(DeleteBehavior.ClientSetNull);
            base.Configure(builder);
        }
    }
}
