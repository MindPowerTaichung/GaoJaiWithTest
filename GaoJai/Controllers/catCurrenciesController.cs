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
    public class catCurrenciesController : ApiController
    {
        private ERPModelContainer db = new ERPModelContainer();

        private CatCurrencyViewModel ToViewModel(catCurrency c)
        {
            return new CatCurrencyViewModel { Id = c.Id, No = c.No, Name = c.Name, TimestampString = Convert.ToBase64String(c.Timestamp) };
        }

        // GET: api/catCurrencies
        public IQueryable<CatCurrencyViewModel> GetcatCurrencies()
        {
            var catcurrencies = db.catCurrencies.ToArray<catCurrency>().Select(f => ToViewModel(f));
            return catcurrencies.AsQueryable();
        }

        // GET: api/catCurrencies/5
        [ResponseType(typeof(CatCurrencyViewModel))]
        public IHttpActionResult GetcatCurrency(int id)
        {
            catCurrency catCurrency = db.catCurrencies.Find(id);
            if (catCurrency == null)
            {
                return NotFound();
            }

            return Ok(ToViewModel(catCurrency));
        }

        // PUT: api/catCurrencies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutcatCurrency(int id, CatCurrencyViewModel Cat_CurrencyView_Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Cat_CurrencyView_Model.Id)
            {
                return BadRequest();
            }

            //把資料庫中的那筆資料讀出來
            var catCurrency_db = db.catCurrencies.Find(id);
            if (catCurrency_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    catCurrency_db.Id = Cat_CurrencyView_Model.Id;
                    catCurrency_db.No = Cat_CurrencyView_Model.No;
                    catCurrency_db.Name = Cat_CurrencyView_Model.Name;
                    db.Entry(catCurrency_db).OriginalValues["Timestamp"] = Convert.FromBase64String(Cat_CurrencyView_Model.TimestampString);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!catCurrencyExists(id))
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));// ""
                }
            }

            return Ok(ToViewModel(catCurrency_db));
        }

        // POST: api/catCurrencies
        [ResponseType(typeof(CatCurrencyViewModel))]
        public IHttpActionResult PostcatCurrency(CatCurrencyViewModel Cat_CurrencyView_Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            catCurrency catCurrency_db;
            try
            {
                catCurrency_db = new catCurrency { Id = Cat_CurrencyView_Model.Id, No = Cat_CurrencyView_Model.No, Name = Cat_CurrencyView_Model.Name };
                db.catCurrencies.Add(catCurrency_db);
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


            return CreatedAtRoute("DefaultApi", new { id = catCurrency_db.Id }, ToViewModel(catCurrency_db));
        }

        // DELETE: api/catCurrencies/5
        [ResponseType(typeof(CatCurrencyViewModel))]
        public IHttpActionResult DeletecatCurrency(int id)
        {
            catCurrency catCurrency = db.catCurrencies.Find(id);
            if (catCurrency == null)
            {
                return NotFound();
            }

            db.catCurrencies.Remove(catCurrency);
            db.SaveChanges();

            return Ok(ToViewModel(catCurrency));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool catCurrencyExists(int id)
        {
            return db.catCurrencies.Count(e => e.Id == id) > 0;
        }
    }
}