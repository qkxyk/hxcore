using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DataDefineWarnCodeModelMap : BaseModelMap<DataDefineWarnCodeModel>
    {
        public override void Configure(EntityTypeBuilder<DataDefineWarnCodeModel> builder)
        {
            builder.ToTable("DataDefineWarnCode").HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}
