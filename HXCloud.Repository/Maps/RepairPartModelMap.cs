using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository
{
    public class RepairPartModelMap : IEntityTypeConfiguration<RepairPartModel>
    {
        public void Configure(EntityTypeBuilder<RepairPartModel> builder)
        {
            builder.ToTable("RepairPart").HasKey(a => a.Id);
            builder.HasOne(a => a.Repair).WithMany(a => a.RepairParts).HasForeignKey(a => a.RepairId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
