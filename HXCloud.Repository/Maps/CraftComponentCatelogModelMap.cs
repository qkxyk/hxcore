using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class CraftComponentCatelogModelMap : BaseModelMap<CraftComponentCatalogModle>
    {
        public override void Configure(EntityTypeBuilder<CraftComponentCatalogModle> builder)
        {
            builder.ToTable("CraftComponentCatelog").HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}
