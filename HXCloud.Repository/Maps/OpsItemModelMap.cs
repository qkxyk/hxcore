using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
   public class OpsItemModelMap:BaseModelMap<OpsItemModel>
    {
        public override void Configure(EntityTypeBuilder<OpsItemModel> builder)
        {
            builder.ToTable("OpsItem").HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}
