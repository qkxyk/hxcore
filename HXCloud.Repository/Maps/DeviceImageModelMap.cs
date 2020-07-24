using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DeviceImageModelMap : BaseModelMap<DeviceImageModel>
    {
        public override void Configure(EntityTypeBuilder<DeviceImageModel> builder)
        {
            builder.ToTable("DeviceImage").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.DeviceImage).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
