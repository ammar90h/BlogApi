using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RubiconProject.Database.Entity
{
    public class Post
    {
        public int PostId { get; set; }

        [Required, Index(IsUnique = true), MaxLength(200)]
        public string Slug { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Body { get; set; }

        public virtual List<Tag> TagList { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}