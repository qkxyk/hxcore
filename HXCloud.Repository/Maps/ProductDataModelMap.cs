using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
  public  class ProductDataModelMap: IEntityTypeConfiguration<ProductDataModel>
    {
        public void Configure(EntityTypeBuilder<ProductDataModel> builder)
        {
            builder.ToTable("ProductData").HasKey(a => a.PatrolId);
            //builder.HasOne(a => a.PatrolData).WithOne(a => a.ProductData).HasForeignKey<ProductDataModel>(k => k.PatrolId).OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
