using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeSystemAccessoyModelMap : BaseModelMap<TypeSystemAccessoryModel>
    {
        public override void Configure(EntityTypeBuilder<TypeSystemAccessoryModel> builder)
        {
            builder.ToTable("TypeSystemAccessory").HasKey(a => a.Id);
            builder.HasOne(a => a.TypeSystem).WithMany(a => a.SystemAccessories).HasForeignKey(a => a.SystemId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
