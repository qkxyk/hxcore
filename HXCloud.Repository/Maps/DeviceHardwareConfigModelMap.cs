using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DeviceHardwareConfigModelMap : BaseModelMap<DeviceHardwareConfigModel>
    {
        public override void Configure(EntityTypeBuilder<DeviceHardwareConfigModel> builder)
        {
            builder.ToTable("DeviceHardwareConfig").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.DeviceHardwareConfig).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
