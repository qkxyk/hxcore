using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository
{ 
    public class DeviceMonitorDataModelMap: IEntityTypeConfiguration<DeviceMonitorDataModel>
    {
        public void Configure(EntityTypeBuilder<DeviceMonitorDataModel> builder)
        {
            builder.ToTable("DeviceMonitorData").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.DeviceMonitorData).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
