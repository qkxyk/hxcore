using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class BaseModelMap<T> : IEntityTypeConfiguration<T> where T : BaseCModel
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(a => a.CreateTime).HasDefaultValueSql("getdate()");
            //builder.Property(a => a.ModifyTime).ValueGeneratedOnAddOrUpdate();
        }
    }
}
