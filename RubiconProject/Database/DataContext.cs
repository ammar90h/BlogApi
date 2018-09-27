using RubiconProject.Database.Entity;
using RubiconProject.Database.Entity.Config;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RubiconProject.Database
{
    public class DataContext : DbContext 
    {
        public DataContext() :base ("PostDb")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new PostConfig());
        }
    }
}