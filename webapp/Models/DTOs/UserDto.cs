using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VPS.Models;

namespace VPS.Models
{
    public class MyUser
    {
        public MyUser()
        { }

        public MyUser(Users user)
        {
            if (user != null)
            {
                this.UserID = user.UserID;
                this.Active = user.Active;
                this.EmailAddress = user.EmailAddress;
                this.Name = user.Name;
                this.Password = "**********";
                this.UserTypeID = user.UserTypeID;

                this.TotalPossession = user.Possessions != null ? user.Possessions.Count : 0;
                //this.TotalPossessionReturn = user.PossessionsReturn != null ? user.PossessionsReturn.Count : 0;
                this.UserType = ((UserTypes)user.UserTypeID).ToString();
            }
        }

        public int UserID { get; set; }

        public int TotalPossession { get; set; }
        public int TotalPossessionReturn { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        //[Required]
        [EmailAddress]
        [Display(Name = "Confirm Email Address")]
        public string ReEmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

       // [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Verify Password ")]
        public string VerifyPassword { get; set; }

        [Required]
        [Display(Name = "User Type")]
        public int UserTypeID { get; set; }

        [Display(Name = "User Type")]
        public string UserType { get; set; }

        [Required]
        public bool Active { get; set; }

    }
}