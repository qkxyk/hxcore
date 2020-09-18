using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeOverviewModelMap : BaseModelMap<TypeOverviewModel>
    {
        public override void Configure(EntityTypeBuilder<TypeOverviewModel> builder)
        {
            builder.ToTable("TypeOverView").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeOverviews).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.TypeDataDefine).WithMany(a => a.TypeOverviews).HasForeignKey(a => a.TypeDataDefineId).OnDelete(DeleteBehavior.ClientSetNull);
            base.Configure(builder);
        }
    }
}
