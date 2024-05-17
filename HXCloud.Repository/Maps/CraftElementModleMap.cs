using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class CraftElementModleMap : BaseModelMap<CraftElementModle>
    {
        public override void Configure(EntityTypeBuilder<CraftElementModle> builder)
        {
            builder.ToTable("CraftElement").HasKey(a => a.Id);
            builder.HasOne(a => a.CraftComponentCatalog).WithMany(a => a.CraftElements).HasForeignKey(a => a.CatalogId)
                .OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
