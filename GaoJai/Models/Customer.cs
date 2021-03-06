//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MPERP2015.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        public int Id { get; set; }
        public Nullable<int> Cat { get; set; }
        public Nullable<int> Agent { get; set; }
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
        public Nullable<int> Currency { get; set; }
        public Nullable<System.DateTime> Lasttrade { get; set; }
        public string Note { get; set; }
        public byte[] Timestamp { get; set; }
    
        public virtual catCurrency catCurrency { get; set; }
        public virtual catCustomer catCustomer { get; set; }
        public virtual catPay catPay { get; set; }
    }
}
