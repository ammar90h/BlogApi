using RubiconProject.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RubiconProject.Database
{
    public class InitDatabase
    {
        public InitDatabase()
        {
            using (var dtx = new DataContext())
            {
                if (!dtx.Database.Exists())
                {
                    // Create database
                    dtx.Database.Initialize(false);

                    // Add Tags
                    var tag1 = new Tag { Name = "IOS" };
                    var tag2 = new Tag { Name = "Android" };
                    dtx.Tags.Add(tag1);
                    dtx.Tags.Add(tag2);

                    // Add Posts
                    var post1 = dtx.Posts.Add(new Post
                    {
                        Slug = "sample-post-1",
                        Title = "Sample Post 1",
                        Description = "Here going sample descriptions...",
                        Body = "Here going post text",
                        TagList = new List<Tag> { tag1, tag2 },
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });

                    // Add post 2
                    var post2 = dtx.Posts.Add(new Post
                    {
                        Slug = "sample-post-2",
                        Title = "Sample Post 2",
                        Description = "Here going sample descriptions...",
                        Body = "Here going post text",
                        TagList = new List<Tag> { tag1 },
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });

                    // Add post 3
                    var post3 = dtx.Posts.Add(new Post
                    {
                        Slug = "sample-post-3",
                        Title = "Sample Post 3",
                        Description = "Here going sample descriptions...",
                        Body = "Here going post text",
                        TagList = new List<Tag> { tag2 },
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });

                    // Save changes
                    dtx.SaveChanges();
                }
            }
        }
    }
}