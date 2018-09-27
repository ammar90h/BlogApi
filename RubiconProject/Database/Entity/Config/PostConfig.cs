using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace RubiconProject.Database.Entity.Config
{
    public class PostConfig : EntityTypeConfiguration<Post>
    {
        public PostConfig()
        {
            HasMany(item => item.TagList).WithMany(item => item.Posts).Map(item => {
                item.MapLeftKey("PostRefId");
                item.MapRightKey("TagRefId");
                item.ToTable("PostTabRel");
            });
        }
    }
}