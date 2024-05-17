using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class OpsFaultModelMap: BaseModelMap<OpsFaultModel>
    {
        public override void Configure(EntityTypeBuilder<OpsFaultModel> builder)
        {
            builder.ToTable("OpsFault").HasKey(a => a.Code);
            builder.HasOne(a => a.OpsFaultType).WithMany(a => a.OpsFalt).HasForeignKey(a => a.OpsFaultTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
