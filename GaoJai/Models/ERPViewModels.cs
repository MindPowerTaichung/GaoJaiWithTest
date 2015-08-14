using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MPERP2015.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string TimestampString { get; set; }
    }

    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int Customer_Id { get; set; }

        public List<OrderDetailViewModel> OrderDetails { get; set; }

        public string TimestampString { get; set; }
    }

    public class OrderDetailViewModel
    {
        public int Order_Id { get; set; }
        public int Quantity { get; set; }
        public int Product_Id { get; set; }
        public string ProductName { get; set; }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public int Category_Id { get; set; }

        public string TimestampString { get; set; }
    }

    public class FactoryViewModel
    {
        public int Id { get; set; }
        public Nullable<int> Cat { get; set; }
        public string CatName { get; set; }
        public Nullable<int> Type { get; set; }
        public string TypeName { get; set; }
        public string NO { get; set; }
        public string Name { get; set; }
        public string Sname { get; set; }
        public string Unid { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Telephone3 { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string Shipaddr { get; set; }
        public string Invoiceaddr { get; set; }
        public Nullable<int> Pay { get; set; }
        public string PayName { get; set; }
        public Nullable<int> Currency { get; set; }
        public string CurrencyName { get; set; }
        public System.DateTime? Lasttrade { get; set; }
        public string Note { get; set; }
        public string TimestampString { get; set; }
    }

    public class CustomerViewModel
    {
        public int Id { get; set; }
        public Nullable<int> Cat { get; set; }
        public string CatName { get; set; }
        public string NO { get; set; }
        public string Name { get; set; }
        public string Sname { get; set; }
        public string Unid { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Telephone3 { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string Shipaddr { get; set; }
        public string Invoiceaddr { get; set; }
        public Nullable<int> Pay { get; set; }
        public string PayName { get; set; }
        public Nullable<int> Currency { get; set; }
        public string CurrencyName { get; set; }
        public System.DateTime? Lasttrade { get; set; }
        public string Note { get; set; }
        public string TimestampString { get; set; }
    }

    public class CatFactoryViewModel
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string TimestampString { get; set; }
    }

    public class TypeFactoryViewModel
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string TimestampString { get; set; }
    }

    public class CatCurrencyViewModel
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string TimestampString { get; set; }
    }

    public class CatPayViewModel
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string TimestampString { get; set; }
    }

    public class CatCustomerViewModel
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string TimestampString { get; set; }
    }
}

