using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class SimbossModelMap : BaseModelMap<SimbossModel>
    {
        public override void Configure(EntityTypeBuilder<SimbossModel> builder)
        {
            builder.ToTable("Simboss").HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}
