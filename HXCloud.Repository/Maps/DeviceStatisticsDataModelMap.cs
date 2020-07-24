using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DeviceStatisticsDataModelMap : BaseModelMap<DeviceStatisticsDataModel>
    {
        public override void Configure(EntityTypeBuilder<DeviceStatisticsDataModel> builder)
        {
            builder.ToTable("DeviceStatisticsData").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.StatisticsData).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
