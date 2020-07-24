using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeStatisticsInfoModelMap : BaseModelMap<TypeStatisticsInfoModel>
    {
        public override void Configure(EntityTypeBuilder<TypeStatisticsInfoModel> builder)
        {
            builder.ToTable("TypeStatisticsInfo").HasKey(a => a.Id);
            builder.Property(a => a.DisplayType).HasDefaultValue(0);
            builder.Property(a => a.ShowState).HasDefaultValue(0);
            builder.Property(a => a.FilterType).HasDefaultValue(0);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeStatisticsInfo).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
