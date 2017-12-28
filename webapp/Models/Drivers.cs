//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VPS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Drivers
    {
        public Drivers()
        {
            this.Possessions = new HashSet<Possessions>();
        }
    
        public int DriverID { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public Nullable<int> Postcode { get; set; }
        public string LicenceNo { get; set; }
        public string MobileNo { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactRelation { get; set; }
        public string EmergencyContactMobileNo { get; set; }
        public string EmergencyContactAddress { get; set; }
        public string UberID { get; set; }
        public string UberIDPassword { get; set; }
        public string EmailAddress { get; set; }
        public string AvatarPath { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual ICollection<Possessions> Possessions { get; set; }
    }
}