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
    public class typeFactoriesController : ApiController
    {
        private ERPModelContainer db = new ERPModelContainer();

        private TypeFactoryViewModel ToViewModel(typeFactory c)
        {
            return new TypeFactoryViewModel { Id = c.Id, No = c.No, Name = c.Name, TimestampString = Convert.ToBase64String(c.Timestamp) };
        }

        // GET: api/typeFactories
        public IQueryable<TypeFactoryViewModel> GettypeFactories()
        {
            var typefactories = db.typeFactories.ToArray<typeFactory>().Select(f => ToViewModel(f));
            return typefactories.AsQueryable();
        }

        // GET: api/typeFactories/5
        [ResponseType(typeof(TypeFactoryViewModel))]
        public IHttpActionResult GettypeFactory(int id)
        {
            typeFactory typeFactory = db.typeFactories.Find(id);
            if (typeFactory == null)
            {
                return NotFound();
            }
            return Ok(ToViewModel(typeFactory));
        }

        // PUT: api/typeFactories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttypeFactory(int id, TypeFactoryViewModel Type_FactoryView_Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Type_FactoryView_Model.Id)
            {
                return BadRequest();
            }

            //把資料庫中的那筆資料讀出來
            var typeFactory_db = db.typeFactories.Find(id);
            if (typeFactory_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    typeFactory_db.Id = Type_FactoryView_Model.Id;
                    typeFactory_db.No = Type_FactoryView_Model.No;
                    typeFactory_db.Name = Type_FactoryView_Model.Name;
                    db.Entry(typeFactory_db).OriginalValues["Timestamp"] = Convert.FromBase64String(Type_FactoryView_Model.TimestampString);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!typeFactoryExists(id))
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));// ""
                }
            }

            return Ok(ToViewModel(typeFactory_db));
        }

        // POST: api/typeFactories
        [ResponseType(typeof(TypeFactoryViewModel))]
        public IHttpActionResult PosttypeFactory(TypeFactoryViewModel Type_FactoryView_Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            typeFactory typeFactory_db;
            try
            {
                typeFactory_db = new typeFactory { Id = Type_FactoryView_Model.Id, No = Type_FactoryView_Model.No, Name = Type_FactoryView_Model.Name };
                db.typeFactories.Add(typeFactory_db);
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


            return CreatedAtRoute("DefaultApi", new { id = typeFactory_db.Id }, ToViewModel(typeFactory_db));
        }

        // DELETE: api/typeFactories/5
        [ResponseType(typeof(TypeFactoryViewModel))]
        public IHttpActionResult DeletetypeFactory(int id)
        {
            typeFactory typeFactory = db.typeFactories.Find(id);
            if (typeFactory == null)
            {
                return NotFound();
            }

            db.typeFactories.Remove(typeFactory);
            db.SaveChanges();

            return Ok(ToViewModel(typeFactory));

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool typeFactoryExists(int id)
        {
            return db.typeFactories.Count(e => e.Id == id) > 0;
        }
    }
}