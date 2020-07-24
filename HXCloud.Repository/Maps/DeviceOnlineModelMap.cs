using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DeviceOnlineModelMap : IEntityTypeConfiguration<DeviceOnlineModel>
    {
        public void Configure(EntityTypeBuilder<DeviceOnlineModel> builder)
        {
            builder.ToTable("DeviceOnline").HasKey(a => a.DeviceSn);
            builder.HasOne(a => a.Device).WithOne(a => a.DeviceOnline).HasForeignKey<DeviceOnlineModel>(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
