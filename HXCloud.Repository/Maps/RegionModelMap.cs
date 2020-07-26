using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class RegionModelMap : BaseModelMap<RegionModel>
    {
        public override void Configure(EntityTypeBuilder<RegionModel> builder)
        {
            builder.ToTable("Region").HasKey(a => new { a.Id, a.GroupId });
            builder.HasOne(a => a.Group).WithMany(a => a.Regions).HasForeignKey(a => a.GroupId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.Parent).WithMany(a => a.Child).HasForeignKey(a => new { a.ParentId, a.GroupId }).IsRequired(false);
            base.Configure(builder);
        }
    }
}
