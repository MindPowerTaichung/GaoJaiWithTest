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
    public class catFactoriesController : ApiController
    {
        private ERPModelContainer db = new ERPModelContainer();

        private CatFactoryViewModel ToViewModel(catFactory c)
        {
            return new CatFactoryViewModel { Id = c.Id, No = c.No, Name = c.Name, TimestampString = Convert.ToBase64String(c.Timestamp) };
        }

        // GET: api/catFactories
        public IQueryable<CatFactoryViewModel> GetcatFactories()
        {
            var catfactories = db.catFactories.ToArray<catFactory>().Select(f => ToViewModel(f));
            return catfactories.AsQueryable();
        }

        // GET: api/catFactories/5
        [ResponseType(typeof(CatFactoryViewModel))]
        public IHttpActionResult GetcatFactory(int id)
        {
            catFactory catFactory = db.catFactories.Find(id);
            if (catFactory == null)
            {
                return NotFound();
            }

            return Ok(ToViewModel(catFactory));

        }

        // PUT: api/catFactories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutcatFactory(int id, CatFactoryViewModel Cat_FactoryView_Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Cat_FactoryView_Model.Id)
            {
                return BadRequest();
            }

            //把資料庫中的那筆資料讀出來
            var catFactory_db = db.catFactories.Find(id);
            if (catFactory_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    catFactory_db.Id = Cat_FactoryView_Model.Id;
                    catFactory_db.No = Cat_FactoryView_Model.No;
                    catFactory_db.Name = Cat_FactoryView_Model.Name;
                    db.Entry(catFactory_db).OriginalValues["Timestamp"] = Convert.FromBase64String(Cat_FactoryView_Model.TimestampString);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!catFactoryExists(id))
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));// ""
                }
            }

            return Ok(ToViewModel(catFactory_db));
        }

        // POST: api/catFactories
        [ResponseType(typeof(CatFactoryViewModel))]
        public IHttpActionResult PostcatFactory(CatFactoryViewModel Cat_FactoryView_Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            catFactory catFactory_db;
            try
            {
                catFactory_db = new catFactory { Id = Cat_FactoryView_Model.Id, No = Cat_FactoryView_Model.No, Name = Cat_FactoryView_Model.Name };
                db.catFactories.Add(catFactory_db);
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


            return CreatedAtRoute("DefaultApi", new { id = catFactory_db.Id }, ToViewModel(catFactory_db));
        }

        // DELETE: api/catFactories/5
        [ResponseType(typeof(CatFactoryViewModel))]
        public IHttpActionResult DeletecatFactory(int id)
        {
            catFactory catFactory = db.catFactories.Find(id);
            if (catFactory == null)
            {
                return NotFound();
            }

            db.catFactories.Remove(catFactory);
            db.SaveChanges();

            return Ok(ToViewModel(catFactory));

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool catFactoryExists(int id)
        {
            return db.catFactories.Count(e => e.Id == id) > 0;
        }
    }
}