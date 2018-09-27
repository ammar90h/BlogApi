using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json.Linq;
using RubiconProject.Database;
using RubiconProject.Database.Entity;
using RubiconProject.Database.Entity.DTO;
using RubiconProject.Helpers;

namespace RubiconProject.Controllers
{
    public class PostsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Posts
        public IHttpActionResult GetPosts(string tag = null)
        {
            IQueryable<Post> query = db.Posts.AsQueryable();

            if (!String.IsNullOrEmpty(tag))
            {
                query = query.Where(item => item.TagList.Where(t => t.Name.Equals(tag)).Count()>0);
            }
            
            List<PostDto> postItems = query
                .Select(p => new PostDto {
                    Slug = p.Slug,
                    Title = p.Title,
                    Description = p.Description,
                    Body = p.Body,
                    TagList = p.TagList.Select(t => t.Name).ToList(),
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).ToList();

            var totalPosts = query.Count();

            return Ok(new {
                blogPosts = postItems,
                postsCount = totalPosts
            });
        }

        // GET: api/Posts/sample-post-1
        [ResponseType(typeof(Post))]
        public IHttpActionResult GetPost(string slug)
        {
            PostDto post = db.Posts
                .Where(item => item.Slug.Equals(slug))
                .Select(p => new PostDto
                {
                    Slug = p.Slug,
                    Title = p.Title,
                    Description = p.Description,
                    Body = p.Body,
                    TagList = p.TagList.Select(t => t.Name).ToList(),
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).FirstOrDefault();

            if (post == null)
            {
                return NotFound();
            }

            return Ok(new {
                blogPost = post
            });
        }

        // PUT: api/Posts/sample-post-1
        public IHttpActionResult PutPost(string slug, [FromBody]JObject value)
        {
            PostDto postDto = value["blogPost"].ToObject<PostDto>();

            Post findPost = db.Posts.Where(p => p.Slug.Equals(slug)).FirstOrDefault();

            if (findPost==null)
            {
                return NotFound();
            }

            // Update post from dto
            if (!String.IsNullOrEmpty(postDto.Slug))
            {
                findPost.Slug = postDto.Slug;
            }

            if (!String.IsNullOrEmpty(postDto.Title))
            {
                findPost.Title = postDto.Title;
            }

            if (!String.IsNullOrEmpty(postDto.Description))
            {
                findPost.Description = postDto.Description;
            }

            if (!String.IsNullOrEmpty(postDto.Body))
            {
                findPost.Body = postDto.Body;
            }

            if (postDto.TagList!=null && postDto.TagList.Count()>0)
            {
                findPost.TagList = InsertTagsToPost(postDto.TagList);
            }

            // Update updated date
            findPost.UpdatedAt = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(findPost).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

                PostDto updatedPost = db.Posts
                .Where(item => item.Slug.Equals(slug))
                .Select(p => new PostDto
                {
                    Slug = p.Slug,
                    Title = p.Title,
                    Description = p.Description,
                    Body = p.Body,
                    TagList = p.TagList.Select(t => t.Name).ToList(),
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).FirstOrDefault();

                return Ok(new
                {
                    blogPost = updatedPost
                });

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(slug))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Posts
        public IHttpActionResult PostPost([FromBody]JObject value)
        {
            PostDto postDto = value["blogPost"].ToObject<PostDto>();
            postDto.Slug = CreateSlug(postDto.Title);

            Post newPost = new Post();
            newPost.Slug = postDto.Slug;
            newPost.Title = postDto.Title;
            newPost.Description = postDto.Description;
            newPost.Body = postDto.Body;
            newPost.TagList = InsertTagsToPost(postDto.TagList);
            newPost.CreatedAt = DateTime.Now;
            newPost.UpdatedAt = DateTime.Now;
            db.Posts.Add(newPost);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SaveChanges();

            return Ok(new
            {
                blogPost = postDto
            });
        }

        // DELETE: api/Posts/sample-post-1
        [ResponseType(typeof(Post))]
        public IHttpActionResult DeletePost(string slug)
        {
            Post post = db.Posts.Where(item => item.Slug.Equals(slug)).FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }

            db.Posts.Remove(post);
            db.SaveChanges();

            return Ok( new { success = true } );
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostExists(string slug)
        {
            return db.Posts.Count(e => e.Slug == slug) > 0;
        }

        public string CreateSlug(string title)
        {
            if (!String.IsNullOrEmpty(title))
            {
                var newSlug = StringHelper.UrlFriendly(title);

                // Check if slug is already used
                bool slugFounded = false;
                int tryCount = 0;
                while (!slugFounded)
                {
                    if (tryCount > 0)
                    {
                        newSlug = newSlug + "-" + tryCount;
                    }

                    // Check slug in db
                    var checkSlug = db.Posts.Where(item => item.Slug.Equals(newSlug)).Count();

                    // Slug is finded
                    if (checkSlug == 0)
                    {
                        slugFounded = true;
                    }
                    tryCount++;
                }

                return newSlug;
            }
            return null;
        }

        private List<Tag> InsertTagsToPost(List<string> tags)
        {
            List<Tag> tagsForInsert = new List<Tag>();
            if (tags!=null)
            {
                
                foreach (var tag in tags)
                {
                    // Check is tag already in database
                    Tag checkTag = db.Tags.Where(t => t.Name.Equals(tag)).FirstOrDefault();
                    if (checkTag != null)
                    {
                        tagsForInsert.Add(checkTag);
                    }
                    else
                    {
                        // Create tag
                        Tag newTag = new Tag { Name = tag };
                        db.Tags.Add(newTag);
                        tagsForInsert.Add(newTag);
                    }
                }
            }
            return tagsForInsert;
        }

    }
}