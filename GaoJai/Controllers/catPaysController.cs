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
    public class catPaysController : ApiController
    {
        private ERPModelContainer db = new ERPModelContainer();

        private CatPayViewModel ToViewModel(catPay c)
        {
            return new CatPayViewModel { Id = c.Id, No = c.No, Name = c.Name, TimestampString = Convert.ToBase64String(c.Timestamp) };
        }

        // GET: api/catPays
        public IQueryable<CatPayViewModel> GetcatPays()
        {
            var catpays = db.catPays.ToArray<catPay>().Select(f => ToViewModel(f));
            return catpays.AsQueryable();
        }

        // GET: api/catPays/5
        [ResponseType(typeof(CatPayViewModel))]
        public IHttpActionResult GetcatPay(int id)
        {
            catPay catPay = db.catPays.Find(id);
            if (catPay == null)
            {
                return NotFound();
            }

            return Ok(ToViewModel(catPay));
        }

        // PUT: api/catPays/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutcatPay(int id, CatPayViewModel Cat_PayView_Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Cat_PayView_Model.Id)
            {
                return BadRequest();
            }

            //把資料庫中的那筆資料讀出來
            var catPay_db = db.catPays.Find(id);
            if (catPay_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    catPay_db.Id = Cat_PayView_Model.Id;
                    catPay_db.No = Cat_PayView_Model.No;
                    catPay_db.Name = Cat_PayView_Model.Name;
                    db.Entry(catPay_db).OriginalValues["Timestamp"] = Convert.FromBase64String(Cat_PayView_Model.TimestampString);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!catPayExists(id))
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));// ""
                }
            }

            return Ok(ToViewModel(catPay_db));
        }

        // POST: api/catPays
        [ResponseType(typeof(CatPayViewModel))]
        public IHttpActionResult PostcatPay(CatPayViewModel Cat_PayView_Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            catPay catPay_db;
            try
            {
                catPay_db = new catPay { Id = Cat_PayView_Model.Id, No = Cat_PayView_Model.No, Name = Cat_PayView_Model.Name };
                db.catPays.Add(catPay_db);
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


            return CreatedAtRoute("DefaultApi", new { id = catPay_db.Id }, ToViewModel(catPay_db));
        }

        // DELETE: api/catPays/5
        [ResponseType(typeof(CatPayViewModel))]
        public IHttpActionResult DeletecatPay(int id)
        {
            catPay catPay = db.catPays.Find(id);
            if (catPay == null)
            {
                return NotFound();
            }

            db.catPays.Remove(catPay);
            db.SaveChanges();

            return Ok(ToViewModel(catPay));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool catPayExists(int id)
        {
            return db.catPays.Count(e => e.Id == id) > 0;
        }
    }
}