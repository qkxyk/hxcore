using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DeviceVideoModelMap : BaseModelMap<DeviceVideoModel>
    {
        public override void Configure(EntityTypeBuilder<DeviceVideoModel> builder)
        {
            builder.ToTable("DeviceVideo").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.DeviceVideo).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
