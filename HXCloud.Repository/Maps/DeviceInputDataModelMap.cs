using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DeviceInputDataModelMap:BaseModelMap<DeviceInputDataModel>
    {
        public override void Configure(EntityTypeBuilder<DeviceInputDataModel> builder)
        {
            builder.ToTable("DeviceInputData").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.DeviceInputData).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
