using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class PlcSecurityModelMap:BaseModelMap<PlcSecurityModel>
    {
        public override void Configure(EntityTypeBuilder<PlcSecurityModel> builder)
        {
            builder.ToTable("PlcSecurity").HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}
