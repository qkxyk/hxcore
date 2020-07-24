using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
   public class DeviceMigrationModelMap:BaseModelMap<DeviceMigrationModel>
    {
        public override void Configure(EntityTypeBuilder<DeviceMigrationModel> builder)
        {
            builder.ToTable("DeviceMigration").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.DeviceMigration).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
