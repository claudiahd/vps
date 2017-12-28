using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VPS.Models.DTOs
{
    public class MyVehicleIssue
    {
        public MyVehicleIssue()
        { }

        public MyVehicleIssue(MyVehicleIssue vehicleIssue)
        {
            if (vehicleIssue != null)
            {

                this.VehicleIssueID = vehicleIssue.VehicleIssueID;
                this.VehicleID = vehicleIssue.VehicleID;
                this.IssueDetails = vehicleIssue.IssueDetails;
                this.ImagePath = vehicleIssue.ImagePath;
                this.ImageThumbnailPath = vehicleIssue.ImageThumbnailPath;
                this.IsIssueFixed = vehicleIssue.IsIssueFixed;
                this.IssueFixeDate = vehicleIssue.IssueFixeDate;


            }
        }
        public int VehicleIssueID { get; set; }
        public int VehicleID { get; set; }

        [Display(Name = "Issue Detail")]
        public string IssueDetails { get; set; }

        [Required]
        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public string ImageThumbnailPath { get; set; }

        [Display(Name = "Issue Reported On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> IssueReportDate { get; set; }

        [Display(Name = "Is Issue Fixed")]
        public bool IsIssueFixed { get; set; }

        [Display(Name = "Issue Fixed On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> IssueFixeDate { get; set; }

        //public virtual Vehicles Vehicles { get; set; }
    }
}