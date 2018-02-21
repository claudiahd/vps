using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VPS.Models;
using System.ComponentModel.DataAnnotations;


namespace VPS.Models.DTOs
{
    public class DriverDto
    {
        public DriverDto()
        { }

        public DriverDto(Drivers driver)
        {
            if (driver != null)
            {
                this.DriverID = driver.DriverID;

                this.Name = driver.GivenName + " " + driver.MiddleName + " " + driver.SurName;
                this.DOB = this.DateOfBirth = driver.DateOfBirth;
                this.Address = driver.Address;
                this.Suburb = driver.Suburb;
                this.State = driver.State;
                this.Postcode = driver.Postcode;
                this.LicenceNo = driver.LicenceNo;
                this.MobileNo = driver.MobileNo;
                this.EmergencyContactName = driver.EmergencyContactName;
                this.EmergencyContactRelation = driver.EmergencyContactRelation;
                this.EmergencyContactMobileNo = driver.EmergencyContactMobileNo;
                this.EmergencyContactAddress = driver.EmergencyContactAddress;
                this.UberID = driver.UberID;
                this.UberIDPassword = driver.UberIDPassword;
                this.EmailAddress = driver.EmailAddress;
                this.GivenName = driver.GivenName;
                this.MiddleName = driver.MiddleName;
                this.SurName = driver.SurName;
                this.IsActive = driver.IsActive == null ? true : (bool)driver.IsActive;
                this.PossessionCount = driver.Possessions.Count;
                this.AvatarPath = driver.AvatarPath;
            }
        }

        [Display(Name = "Name")]
        public String Name { get; set; }

        [Display(Name = "Given Name")]
        public string GivenName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Sur Name")]
        public string SurName { get; set; }

        public int DriverID { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public System.DateTime DateOfBirth { get; set; }

        [Display(Name = "DOB")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public System.DateTime DOB { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Suburb")]
        public string Suburb { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Postcode")]
        [DataType(DataType.PostalCode)]
        public Nullable<int> Postcode { get; set; }

        [Display(Name = "Licence No")]
        public string LicenceNo { get; set; }

        [Display(Name = "Mobile No")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }

        [Display(Name = "Refferal Name")]
        public string EmergencyContactName { get; set; }

        [Display(Name = "Emergency Contact Relation")]
        public string EmergencyContactRelation { get; set; }

        [Display(Name = "Emergency Mobile No")]
        [DataType(DataType.PhoneNumber)]
        public string EmergencyContactMobileNo { get; set; }

        [Display(Name = "Emergency Contact Address")]
        public string EmergencyContactAddress { get; set; }

        [Display(Name = "Uber ID")]
        public string UberID { get; set; }

        [Display(Name = "Uber ID password")]
        [DataType(DataType.Text)]
        public string UberIDPassword { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Total Possessions")]
        public int PossessionCount { get; set; }

        public string AvatarPath { get; set; }
    }
}