using HXCloud.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
   public class RepairModelMap:BaseModelMap<RepairModel>
    {
        public override void Configure(EntityTypeBuilder<RepairModel> builder)
        {
            builder.Property(a => a.RepairStatus).HasConversion<string>();
            builder.Property(a => a.RepairType).HasConversion<string>();
            builder.Property(a => a.EmergenceStatus).HasConversion<string>();
            base.Configure(builder);
        }
    }
}
