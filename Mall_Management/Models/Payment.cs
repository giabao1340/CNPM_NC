//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mall_Management.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Payment
    {
        public int PaymentID { get; set; }
        public int ContractID { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    
        public virtual Contract Contract { get; set; }
    }
}
