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
            base.Configure(builder);
        }
    }
}
