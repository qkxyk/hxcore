using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class OpsFaultTypeModelMap:BaseModelMap<OpsFaultTypeModel>
    {
        public override void Configure(EntityTypeBuilder<OpsFaultTypeModel> builder)
        {
            builder.ToTable("OpsFaultType").HasKey(a => a.FaultTypeId);
            builder.HasOne(a => a.Parent).WithMany(a => a.Child).HasForeignKey(a=>a.ParentId).IsRequired(false);
            base.Configure(builder);
        }
    }
}
