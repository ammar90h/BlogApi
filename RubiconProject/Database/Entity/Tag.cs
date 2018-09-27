using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RubiconProject.Database.Entity
{
    public class Tag
    {
        public int TagId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual List<Post> Posts { get; set; }
    }
}