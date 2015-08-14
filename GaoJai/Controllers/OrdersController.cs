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
    public class OrdersController : ApiController
    {
        private ERPModelContainer db = new ERPModelContainer();

        OrderViewModel toOrderViewModel(Order order)
        {
            var orderViewModel = new OrderViewModel {   Id=order.Id, 
                                                                                Customer_Id=order.Customer_Id, 
                                                                                OrderDate=order.OrderDate,
                                                                                TimestampString=Convert.ToBase64String(order.Timestamp) };

            orderViewModel.OrderDetails = new List<OrderDetailViewModel>();
            foreach (var detail in order.OrderDetails)
            {
                orderViewModel.OrderDetails.Add(new OrderDetailViewModel
                {
                    Order_Id = order.Id,
                    Product_Id = detail.Product_Id,
                    ProductName = detail.Product.Name,
                    Quantity = detail.Quantity
                });
            }
            return orderViewModel;
        }

        // GET: api/Orders
        public IQueryable<OrderViewModel> GetOrders()
        {
            var orders = db.Orders.ToArray<Order>().Select(o => toOrderViewModel(o));
            return orders.AsQueryable();
        }

        // GET: api/Orders/5
        [ResponseType(typeof(OrderViewModel))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(toOrderViewModel(order));
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, OrderViewModel order_view_model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order_view_model.Id)
            {
                return BadRequest();
            }

            //把資料庫中的那筆資料讀出來
            var order = db.Orders.Find(order_view_model.Id);
            if (order == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
            }
            else
            {
                try
                {
                    order.OrderDate = order_view_model.OrderDate;
                    order.Customer_Id = order_view_model.Customer_Id;
                    order.OrderDetails.Clear();
                    Product product;
                    foreach (var detail in order_view_model.OrderDetails)
                    {
                        product = db.Products.Find(detail.Product_Id);
                        order.OrderDetails.Add(new OrderDetail { Product = product, Quantity = detail.Quantity });
                    }
                    db.Entry(order).OriginalValues["Timestamp"] = Convert.FromBase64String(order_view_model.TimestampString);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(id))
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, "這筆資料已被刪除!"));
                    else
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict, "這筆資料已被其他人修改!"));// ""
                }
            }

            return Ok(toOrderViewModel(order));
        }

        // POST: api/Orders
        [ResponseType(typeof(OrderViewModel))]
        public IHttpActionResult PostOrder(OrderViewModel order_view_model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            Order order = new Order { Customer_Id=order_view_model.Customer_Id, OrderDate=DateTime.Now };
            //order.OrderDetails = new List<OrderDetail>();
            Product product;
            foreach (var detail in order_view_model.OrderDetails)
            {
                product = db.Products.Find(detail.Product_Id);
                order.OrderDetails.Add(new OrderDetail { Product=product, Quantity=detail.Quantity });
            }
            db.Orders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(OrderViewModel))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            //return Ok(toOrderViewModel(order));
            return Ok(new OrderViewModel { Id=id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}