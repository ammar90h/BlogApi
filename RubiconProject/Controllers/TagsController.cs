using RubiconProject.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RubiconProject.Controllers
{
    public class TagsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET api/tags
        public IHttpActionResult Get()
        {
            List<string> tags = db.Tags.Select(t => t.Name).ToList();
            return Ok(new { tags });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
