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
using MPERP2015.Models;
using System.Security.Claims;

namespace MPERP2015.Controllers
{
    //[Authorize]
    public class CategoriesController : ApiController
    {
        private ERPModelContainer db = new ERPModelContainer();

        private CategoryViewModel ToViewModel(Category c)
        {
            return new CategoryViewModel { Id = c.Id, Name = c.Name, TimestampString=Convert.ToBase64String( c.Timestamp ) };
        }

        // GET: api/Categories
        public IQueryable<CategoryViewModel> GetCategories()
        {
            var identity = User.Identity as ClaimsIdentity;

            var claims = from c in identity.Claims
                         select new
                         {
                             subject = c.Subject.Name,
                             type = c.Type,
                             value = c.Value
                         };

            var categories= db.Categories.ToArray<Category>().Select(c=>ToViewModel(c));
            return categories.AsQueryable();
        }

        // GET: api/Categories/5
        [ResponseType(typeof(CategoryViewModel))]
        public IHttpActionResult GetCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(ToViewModel(category));
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(CategoryViewModel))]
        public IHttpActionResult PutCategory(int id, CategoryViewModel category_view_model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != category_view_model.Id)
                return BadRequest();

            //把資料庫中的那筆資料讀出來
            var category_db = db.Categories.Find(id);
            if (category_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    category_db.Name = category_view_model.Name;
                    db.Entry(category_db).OriginalValues["Timestamp"] = Convert.FromBase64String(category_view_model.TimestampString);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(id))
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));// ""
                }
            }

            return Ok(ToViewModel(category_db));

        }

        // POST: api/Categories
        [ResponseType(typeof(CategoryViewModel))]
        public IHttpActionResult PostCategory([FromBody]CategoryViewModel category_view_model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Category category = new Category { Id = category_view_model.Id, Name = category_view_model.Name };
            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.Id }, ToViewModel(category));
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(CategoryViewModel))]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            //return Ok(ToViewModel(category));
            return Ok(new CategoryViewModel { Id=id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.Id == id) > 0;
        }
    }
}