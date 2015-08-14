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
    public class CustomersController : ApiController
    {
        private ERPModelContainer db = new ERPModelContainer();

        private IQueryable<CustomerViewModel> ToViewModels(Customer[] customers)
        {
            return customers.Select(c => ToViewModel(c)).AsQueryable();
        }
        private CustomerViewModel ToViewModel(Customer c)
        {
            catCustomer catCustomer = c.catCustomer;
            if (catCustomer == null)
            {
                catCustomer = db.catCustomers.Find(c.Cat);
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
            return new CustomerViewModel { Id = c.Id, Cat = c.Cat,
                                           CatName = catCustomer == null ? "" : catCustomer.Name,
                                           PayName = catPay == null ? "" : catPay.Name,
                                           CurrencyName = catCurrency == null ? "" : catCurrency.Name,
                                           Currency=c.Currency,
                                           Pay=c.Pay,
                                           NO = c.NO,
                                           Name = c.Name,
                                           Sname = c.Sname,
                                           Unid = c.Unid,
                                           Contact1 = c.Contact1,
                                           Contact2 = c.Contact2,
                                           Email1 = c.Email1,
                                           Email2 = c.Email2,
                                           Email3 = c.Email3,
                                           Telephone1 = c.Telephone1,
                                           Telephone2 = c.Telephone2,
                                           Telephone3 = c.Telephone3,
                                           Website = c.Website,
                                           Fax = c.Fax,
                                           Address = c.Address,
                                           Shipaddr = c.Shipaddr,
                                           Invoiceaddr = c.Invoiceaddr,
                                           Lasttrade = c.Lasttrade,
                                           Note = c.Note,
                                           TimestampString = Convert.ToBase64String(c.Timestamp)
            };
        }

        // GET: api/Customers
        public IQueryable<CustomerViewModel> GetCustomers()
        {
            var customers = db.Customers.ToArray<Customer>().Select(f => ToViewModel(f));
            return customers.AsQueryable();
        }

        // GET: api/Customers/5
        [ResponseType(typeof(CustomerViewModel))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(ToViewModel(customer));
        }

        // GET: api/Customers/5
        [Route("api/Customers/phone/{no?}")]
        [ResponseType(typeof(CustomerViewModel))]
        public IQueryable<CustomerViewModel> GetCustomerByPhone(string no=null)
        {
            Customer[] customers;
            if (string.IsNullOrWhiteSpace(no))
            {
                var query = from c in db.Customers
                            select c;
                customers = query.ToArray<Customer>();
            }
            else
            {
                var query = from c in db.Customers
                            where c.Telephone1.Contains(no) || c.Telephone2.Contains(no) || c.Telephone3.Contains(no)
                            select c;
                customers = query.ToArray<Customer>();
            }

            return ToViewModels(customers);
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, CustomerViewModel customer_View_Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer_View_Model.Id)
            {
                return BadRequest();
            }

            //把資料庫中的那筆資料讀出來
            var customer_db = db.Customers.Find(id);
            if (customer_db == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    //customer_db.catCustomer.Name = customer_View_Model.CatName;
                    customer_db.Cat = customer_View_Model.Cat;
                    customer_db.NO = customer_View_Model.NO;
                    customer_db.Name = customer_View_Model.Name;
                    customer_db.Sname = customer_View_Model.Sname;
                    customer_db.Unid = customer_View_Model.Unid;
                    customer_db.Contact1 = customer_View_Model.Contact1;
                    customer_db.Contact2 = customer_View_Model.Contact2;
                    customer_db.Email1 = customer_View_Model.Email1;
                    customer_db.Email2 = customer_View_Model.Email2;
                    customer_db.Email3 = customer_View_Model.Email3;
                    customer_db.Telephone1 = customer_View_Model.Telephone1;
                    customer_db.Telephone2 = customer_View_Model.Telephone2;
                    customer_db.Telephone3 = customer_View_Model.Telephone3;
                    customer_db.Website = customer_View_Model.Website;
                    customer_db.Fax = customer_View_Model.Fax;
                    customer_db.Address = customer_View_Model.Address;
                    customer_db.Shipaddr = customer_View_Model.Shipaddr;
                    customer_db.Invoiceaddr = customer_View_Model.Invoiceaddr;
                    customer_db.Pay = customer_View_Model.Pay;
                    //customer_db.catPay.Name = customer_View_Model.PayName;
                    customer_db.Currency = customer_View_Model.Currency;
                    //customer_db.catCurrency.Name = customer_View_Model.CurrencyName;
                    customer_db.Lasttrade = customer_View_Model.Lasttrade;
                    customer_db.Note = customer_View_Model.Note;
                    db.Entry(customer_db).OriginalValues["Timestamp"] = Convert.FromBase64String(customer_View_Model.TimestampString);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(id))
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));// ""
                }
            }

            return Ok(ToViewModel(customer_db));
        }

        // POST: api/Customers
        [ResponseType(typeof(CustomerViewModel))]
        public IHttpActionResult PostCustomer(CustomerViewModel customer_View_Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customer_db;
            try
            {
                customer_db = new Customer { Id = customer_View_Model.Id, Cat = customer_View_Model.Cat, NO = customer_View_Model.NO, Name = customer_View_Model.Name, Sname = customer_View_Model.Sname, Unid = customer_View_Model.Unid, Contact1 = customer_View_Model.Contact1, Contact2 = customer_View_Model.Contact2, Email1 = customer_View_Model.Email1, Email2 = customer_View_Model.Email2, Email3 = customer_View_Model.Email3, Telephone1 = customer_View_Model.Telephone1, Telephone2 = customer_View_Model.Telephone2, Telephone3 = customer_View_Model.Telephone3, Website = customer_View_Model.Website, Fax = customer_View_Model.Fax, Address = customer_View_Model.Address, Shipaddr = customer_View_Model.Shipaddr, Invoiceaddr = customer_View_Model.Invoiceaddr, Pay = customer_View_Model.Pay, Currency = customer_View_Model.Currency, Lasttrade = customer_View_Model.Lasttrade, Note = customer_View_Model.Note };
                db.Customers.Add(customer_db);
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


            return CreatedAtRoute("DefaultApi", new { id = customer_db.Id }, ToViewModel(customer_db));
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(CustomerViewModel))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(ToViewModel(customer));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.Id == id) > 0;
        }
    }
}