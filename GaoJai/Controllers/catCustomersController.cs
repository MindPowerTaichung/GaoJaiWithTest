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
using MPERP2015.Models;

namespace MPERP2015.Controllers
{
    public class catCustomersController : ApiController
    {
        private ERPModelContainer db = new ERPModelContainer();

        private CatCustomerViewModel ToViewModel(catCustomer c)
        {
            return new CatCustomerViewModel { Id = c.Id, No = c.No, Name = c.Name, TimestampString = Convert.ToBase64String(c.Timestamp) };
        }

        // GET: api/catCustomers
        public IQueryable<CatCustomerViewModel> GetcatCustomers()
        {
            var catcustomers = db.catCustomers.ToArray<catCustomer>().Select(f => ToViewModel(f));
            return catcustomers.AsQueryable();
        }

        // GET: api/catCustomers/5
        [ResponseType(typeof(CatCustomerViewModel))]
        public IHttpActionResult GetcatCustomer(int id)
        {
            catCustomer catCustomer = db.catCustomers.Find(id);
            if (catCustomer == null)
            {
                return NotFound();
            }

            return Ok(ToViewModel(catCustomer));
        }

        // PUT: api/catCustomers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutcatCustomer(int id, CatCustomerViewModel Cat_CustomerView_Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Cat_CustomerView_Model.Id)
            {
                return BadRequest();
            }

            //把資料庫中的那筆資料讀出來
            var catCustomer_db = db.catCustomers.Find(id);
            if (catCustomer_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    catCustomer_db.Id = Cat_CustomerView_Model.Id;
                    catCustomer_db.No = Cat_CustomerView_Model.No;
                    catCustomer_db.Name = Cat_CustomerView_Model.Name;
                    db.Entry(catCustomer_db).OriginalValues["Timestamp"] = Convert.FromBase64String(Cat_CustomerView_Model.TimestampString);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!catCustomerExists(id))
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));// ""
                }
            }

            return Ok(ToViewModel(catCustomer_db));
        }

        // POST: api/catCustomers
        [ResponseType(typeof(CatCustomerViewModel))]
        public IHttpActionResult PostcatCustomer(CatCustomerViewModel Cat_CustomerView_Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            catCustomer catCustomer_db;
            try
            {
                catCustomer_db = new catCustomer { Id = Cat_CustomerView_Model.Id, No = Cat_CustomerView_Model.No, Name = Cat_CustomerView_Model.Name };
                db.catCustomers.Add(catCustomer_db);
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, "DbEntityValidationException:" + ex.Message));
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message));
            }


            return CreatedAtRoute("DefaultApi", new { id = catCustomer_db.Id }, ToViewModel(catCustomer_db));
        }

        // DELETE: api/catCustomers/5
        [ResponseType(typeof(CatCustomerViewModel))]
        public IHttpActionResult DeletecatCustomer(int id)
        {
            catCustomer catCustomer = db.catCustomers.Find(id);
            if (catCustomer == null)
            {
                return NotFound();
            }

            db.catCustomers.Remove(catCustomer);
            db.SaveChanges();

            return Ok(ToViewModel(catCustomer));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool catCustomerExists(int id)
        {
            return db.catCustomers.Count(e => e.Id == id) > 0;
        }
    }
}