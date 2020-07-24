using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
 public   class AppVersionModelMap:BaseModelMap<AppVersionModel>
    {
        public override void Configure(EntityTypeBuilder<AppVersionModel> builder)
        {
            builder.ToTable("AppVersion").HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}
