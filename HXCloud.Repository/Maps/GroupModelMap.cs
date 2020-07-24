using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class GroupModelMap : BaseModelMap<GroupModel>
    {
        public override void Configure(EntityTypeBuilder<GroupModel> builder)
        {
            //builder.Property(a => a.EditTime).ValueGeneratedOnAddOrUpdate();
            //builder.Property(a => a.CreateTime).HasDefaultValueSql("getdate()");
            builder.ToTable("Group").HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}
