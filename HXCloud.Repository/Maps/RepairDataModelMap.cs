using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository
{
    public class RepairDataModelMap : IEntityTypeConfiguration<RepairDataModel>
    {
        public void Configure(EntityTypeBuilder<RepairDataModel> builder)
        {
            builder.ToTable("RepairData").HasKey(a => a.Id);
            builder.Property(a => a.RepairStatus).HasConversion<string>();
            builder.HasOne(a => a.Repair).WithMany(a => a.RepairDatas).HasForeignKey(a => a.RepairId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
