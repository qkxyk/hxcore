using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DeviceLogModelMap : BaseModelMap<DeviceLogModel>
    {
        public override void Configure(EntityTypeBuilder<DeviceLogModel> builder)
        {
            builder.ToTable("DeviceLog").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.DeviceLog).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
