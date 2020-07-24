using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DataDefineLibraryModelMap : BaseModelMap<DataDefineLibraryModel>
    {
        public override void Configure(EntityTypeBuilder<DataDefineLibraryModel> builder)
        {
            builder.ToTable("DataDefineLibrary").HasKey(a => a.Id);
            builder.Property(a => a.Model).HasDefaultValue(0);
            base.Configure(builder);
        }
    }
}
