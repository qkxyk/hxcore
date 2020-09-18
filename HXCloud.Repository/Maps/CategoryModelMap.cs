using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class CategoryModelMap:BaseModelMap<CategoryModel>
    {
        public override void Configure(EntityTypeBuilder<CategoryModel> builder)
        {
            builder.ToTable("Category").HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}
