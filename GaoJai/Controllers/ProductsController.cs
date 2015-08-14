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

namespace MPERP2015.Controllers
{
    public class ProductsController : ApiController
    {
        private ERPModelContainer db = new ERPModelContainer();
        private ProductViewModel ToViewModel(Product p)
        {
            return new ProductViewModel { 
                Id = p.Id, 
                Name = p.Name, 
                Price = p.Price, 
                Category_Id =p.Category==null? 0:p.Category.Id,
                TimestampString=Convert.ToBase64String( p.Timestamp )
            };
        }

        // GET: api/Products
        public IQueryable<ProductViewModel> GetProducts()
        {
            var products = db.Products.ToArray<Product>().Select(p => ToViewModel(p));
            return products.AsQueryable();
        }

        // GET: api/ProductsByCategory/{id}
        [Route("api/ProductsByCategory/{id}")]
        public IQueryable<ProductViewModel> GetProductsByCategory(int id)
        {
            var products = db.Products.Where(p => p.Category.Id==id).ToArray<Product>().Select(p => ToViewModel(p));
            return products.AsQueryable();
        }

        // GET: api/Products/5
        [ResponseType(typeof(ProductViewModel))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(ToViewModel(product));
        }

        // PUT: api/Products/5
        [ResponseType(typeof(ProductViewModel))]
        public IHttpActionResult PutProduct(int id, ProductViewModel product_view_model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product_view_model.Id)
            {
                return BadRequest();
            }
            
            Category category = db.Categories.Find(product_view_model.Category_Id);
            if (category == null)
            {
                return BadRequest("找不到對應的產品分類: " + product_view_model.Category_Id);
            }

            //把資料庫中的那筆資料讀出來
            Product product_db = db.Products.Find(id);
            if (product_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    product_db.Name = product_view_model.Name;
                    product_db.Price = product_view_model.Price;
                    product_db.Category = category;
                    db.Entry(product_db).OriginalValues["Timestamp"] = Convert.FromBase64String(product_view_model.TimestampString);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));
                }
            }

            return Ok(ToViewModel(product_db)); 
        }

        // POST: api/Products
        [ResponseType(typeof(ProductViewModel))]
        public IHttpActionResult PostProduct(ProductViewModel product_view_model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = db.Categories.Find(product_view_model.Category_Id);
            if (category==null)
            {
                return BadRequest("找不到對應的產品分類: " + product_view_model.Category_Id);
            }
            Product product = new Product { Name = product_view_model.Name, Price = product_view_model.Price, Category = category };
            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, ToViewModel(product));
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(new ProductViewModel { Id=id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}