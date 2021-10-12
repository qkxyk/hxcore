using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DeviceDayMonitorDataModelMap : IEntityTypeConfiguration<DeviceDayMonitorDataModel>
    {
        public void Configure(EntityTypeBuilder<DeviceDayMonitorDataModel> builder)
        {
            builder.ToTable("DeviceDayMonitorData").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.DeviceMonitorData_Day).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
        }
    }


}
