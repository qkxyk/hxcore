using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DeviceDiscreteStatisticsDataModelMap : IEntityTypeConfiguration<DeviceDiscreteStatisticsDataModel>
    {
        public void Configure(EntityTypeBuilder<DeviceDiscreteStatisticsDataModel> builder)
        {
            builder.ToTable("DeviceDiscreteStatisticsData").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.DiscreteStatisticsData).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
