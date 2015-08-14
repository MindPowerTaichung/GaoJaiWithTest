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
    public class FactoriesController : ApiController
    {
        private ERPModelContainer db = new ERPModelContainer();

        private FactoryViewModel ToViewModel(Factory c)
        {
            catFactory catFactory=c.catFactory;
            if (catFactory==null)
            {
                catFactory = db.catFactories.Find(c.Cat);
            }
            typeFactory typeFactory = c.typeFactory;
            if (typeFactory == null)
            {
                typeFactory = db.typeFactories.Find(c.Type);
            }
            catPay catPay = c.catPay;
            if (catPay == null)
            {
                catPay = db.catPays.Find(c.Pay);
            }
            catCurrency catCurrency = c.catCurrency;
            if (catCurrency == null)
            {
                catCurrency = db.catCurrencies.Find(c.Currency);
            }

            return new FactoryViewModel { Id = c.Id, Cat = c.Cat, 
                CatName = catFactory == null ? "" : catFactory.Name, 
                TypeName = typeFactory==null ? "": typeFactory.Name,
                PayName = catPay == null ? "" : catPay.Name,
                CurrencyName = catCurrency == null ? "" : catCurrency.Name,
                Type = c.Type, NO = c.NO, Name = c.Name, Sname = c.Sname, Unid = c.Unid, Contact1 = c.Contact1, Contact2 = c.Contact2, Email1 = c.Email1, Email2 = c.Email2, Email3 = c.Email3, Telephone1 = c.Telephone1, Telephone2 = c.Telephone2, Telephone3 = c.Telephone3, Website = c.Website, Fax = c.Fax, Address = c.Address, Shipaddr = c.Shipaddr, Invoiceaddr = c.Invoiceaddr, Pay = c.Pay, Currency = c.Currency, Lasttrade = c.Lasttrade, Note = c.Note, TimestampString = Convert.ToBase64String(c.Timestamp) };
        }

        // GET: api/Factories
        public IQueryable<FactoryViewModel> GetFactories()
        {
            var factories = db.Factories.ToArray<Factory>().Select(f => ToViewModel(f));
            return factories.AsQueryable();
        }

        // GET: api/Factories/5
        [ResponseType(typeof(FactoryViewModel))]
        public IHttpActionResult GetFactory(int id)
        {
            Factory factory = db.Factories.Find(id);
            if (factory == null)
            {
                return NotFound();
            }
            return Ok(ToViewModel(factory));
        }

        // PUT: api/Factories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFactory(int id, FactoryViewModel factory_view_model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != factory_view_model.Id)
            {
                return BadRequest();
            }

            //把資料庫中的那筆資料讀出來
            var factory_db = db.Factories.Find(id);
            if (factory_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    //factory_db.catFactory.Name = factory_view_model.CatName;
                    factory_db.Cat = factory_view_model.Cat;
                    //factory_db.typeFactory.Name = factory_view_model.TypeName;
                    factory_db.Type = factory_view_model.Type;
                    factory_db.NO = factory_view_model.NO;
                    factory_db.Name = factory_view_model.Name;
                    factory_db.Sname = factory_view_model.Sname;
                    factory_db.Unid = factory_view_model.Unid;
                    factory_db.Contact1 = factory_view_model.Contact1;
                    factory_db.Contact2 = factory_view_model.Contact2;
                    factory_db.Email1 = factory_view_model.Email1;
                    factory_db.Email2 = factory_view_model.Email2;
                    factory_db.Email3 = factory_view_model.Email3;
                    factory_db.Telephone1 = factory_view_model.Telephone1;
                    factory_db.Telephone2 = factory_view_model.Telephone2;
                    factory_db.Telephone3 = factory_view_model.Telephone3;
                    factory_db.Website = factory_view_model.Website;
                    factory_db.Fax = factory_view_model.Fax;
                    factory_db.Address = factory_view_model.Address;
                    factory_db.Shipaddr = factory_view_model.Shipaddr;
                    factory_db.Invoiceaddr = factory_view_model.Invoiceaddr;
                    factory_db.Pay = factory_view_model.Pay;
                    //factory_db.catPay.Name = factory_view_model.PayName;
                    factory_db.Currency = factory_view_model.Currency;
                    //factory_db.catCurrency.Name = factory_view_model.CurrencyName;
                    factory_db.Lasttrade = factory_view_model.Lasttrade;
                    factory_db.Note = factory_view_model.Note;
                    db.Entry(factory_db).OriginalValues["Timestamp"] = Convert.FromBase64String(factory_view_model.TimestampString);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactoryExists(id))
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));// ""
                }
            }

            return Ok(ToViewModel(factory_db));

        }

        // POST: api/Factories
        [ResponseType(typeof(FactoryViewModel))]
        public IHttpActionResult PostFactory(FactoryViewModel factory_view_model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Factory factory_db ;
            try
            {
                factory_db = new Factory { Id = factory_view_model.Id, Cat = factory_view_model.Cat, Type = factory_view_model.Type, NO = factory_view_model.NO, Name = factory_view_model.Name, Sname = factory_view_model.Sname, Unid = factory_view_model.Unid, Contact1 = factory_view_model.Contact1, Contact2 = factory_view_model.Contact2, Email1 = factory_view_model.Email1, Email2 = factory_view_model.Email2, Email3 = factory_view_model.Email3, Telephone1 = factory_view_model.Telephone1, Telephone2 = factory_view_model.Telephone2, Telephone3 = factory_view_model.Telephone3, Website = factory_view_model.Website, Fax = factory_view_model.Fax, Address = factory_view_model.Address, Shipaddr = factory_view_model.Shipaddr, Invoiceaddr = factory_view_model.Invoiceaddr, Pay = factory_view_model.Pay, Currency = factory_view_model.Currency, Lasttrade = factory_view_model.Lasttrade, Note = factory_view_model.Note };
                db.Factories.Add(factory_db);
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


            return CreatedAtRoute("DefaultApi", new { id = factory_db.Id }, ToViewModel(factory_db));
        }

        // DELETE: api/Factories/5
        [ResponseType(typeof(FactoryViewModel))]
        public IHttpActionResult DeleteFactory(int id)
        {
            Factory factory = db.Factories.Find(id);
            if (factory == null)
            {
                return NotFound();
            }

            db.Factories.Remove(factory);
            db.SaveChanges();

            return Ok(ToViewModel(factory));

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FactoryExists(int id)
        {
            return db.Factories.Count(e => e.Id == id) > 0;
        }
    }
}