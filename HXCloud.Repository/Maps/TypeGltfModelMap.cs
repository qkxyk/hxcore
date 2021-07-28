using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeGltfModelMap : BaseModelMap<TypeGltfModel>
    {
        public override void Configure(EntityTypeBuilder<TypeGltfModel> builder)
        {
            builder.ToTable("TypeGltf").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithOne(a => a.TypeGltf).HasForeignKey<TypeGltfModel>(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
