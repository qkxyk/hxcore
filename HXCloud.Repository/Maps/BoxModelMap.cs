using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class BoxModelMap : BaseModelMap<BoxModel>
    {
        public override void Configure(EntityTypeBuilder<BoxModel> builder)
        {
            builder.ToTable("HXBox").HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}
