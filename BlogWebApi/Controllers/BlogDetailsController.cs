using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Blog.DataBase;
using BlogWebApi.Attributes;

namespace BlogWebApi.Controllers
{
    [BasicAuthentication]
    public class BlogDetailsController : ApiController
    {
        private BlogEntities db = new BlogEntities();

        // GET: api/BlogDetails
        public IHttpActionResult GetBlogDetails()
        {
            try 
            { 
                return Ok(db.BlogDetails);
            } 
            catch (Exception ex)
            { 
                
                return Ok(ex.Message + ex.InnerException);
            }
        }

        // GET: api/BlogDetails/5
        [ResponseType(typeof(BlogDetail))]
        public IHttpActionResult GetBlogDetail(int id)
        {
            try
            {
                BlogDetail blogDetail = db.BlogDetails.Find(id);
                
                if (blogDetail == null)
                {
                    return Ok("No blog is present with selected Id");
                }

                return Ok(blogDetail);
            } 
            catch (Exception ex) 
            {
                return Ok(ex.Message + ex.InnerException);
            }
           
        }

        // PUT: api/BlogDetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBlogDetail(int id, BlogDetail blogDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blogDetail.BlogId)
            {
                return Ok("No blog is present with selected Id");
            }

            try {

                var details = db.BlogDetails.Where(x => x.BlogId == id).First();

                if (details != null)
                {
                    details.BlogType = blogDetail.BlogType;
                    details.BlogTitle = blogDetail.BlogTitle;
                    details.BlogContent = blogDetail.BlogContent;
                    details.DateModified = DateTime.Now;

                    db.SaveChanges();
                }
                return Ok("Blog has been updated");
            } 
            catch (Exception ex) 
            { 
                return Ok(ex.Message + ex.InnerException);
            }

        }

        // POST: api/BlogDetails
        [HttpPost]
        [ResponseType(typeof(BlogDetail))]
        public IHttpActionResult PostBlogDetail(BlogDetail blogDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (blogDetail != null)
                {
                    var details = new BlogDetail
                    {
                        BlogTitle = blogDetail.BlogTitle,
                        BlogContent = blogDetail.BlogContent,
                        BlogType = blogDetail.BlogType,
                        CreatedBy = blogDetail.CreatedBy,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        IsDeleted = false
                    };

                    db.BlogDetails.Add(details);
                    db.SaveChanges();

                    return Ok("Blog successfully created");
                }

                return Ok("Blog Details are empty");

            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.InnerException);
            }
        }

        // DELETE: api/BlogDetails/5
        [ResponseType(typeof(BlogDetail))]
        public IHttpActionResult DeleteBlogDetail(int id)
        {
            try {
                var record = db.BlogDetails.Where(x => x.BlogId == id).FirstOrDefault();

                if (record == null)
                {
                    return Ok("No blog is present with selected Id");
                }

                record.IsDeleted = true;
                record.DateModified = DateTime.Now;

                db.SaveChanges();

                return Ok("Blog is deleted successfully");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.InnerException);
            }
           
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