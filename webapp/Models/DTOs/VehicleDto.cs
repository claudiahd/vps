using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using VPS.Models;

namespace VPS.Models.DTOs
{

    public class VehicleDto
    {
        public VehicleDto()
        { }
        public VehicleDto(Vehicles vehicle)
        {
            if (vehicle != null)
            {
                this.VehicleID = vehicle.VehicleID;
                this.RegistrationNo = vehicle.RegistrationNo;
                this.Color = vehicle.Color;
                this.Make = vehicle.Make;
                this.Model = vehicle.Model;
                this.RegoExpiryDate = this.RegistrationExpiryDate = vehicle.RegistrationExpiryDate.Value.Date;
                this.FirstODOMeterReading = vehicle.FirstODOMeterReading;
                this.LastODOMeterReading = vehicle.LastODOMeterReading;
                this.EngineServiceDueKM = vehicle.EngineServiceDueKM;
                this.TransmissionServiceDueKM = vehicle.TransmissionServiceDueKM;
                this.Year = vehicle.Year;
                this.ETagHolder = vehicle.ETagHolder == null ? false : (bool)vehicle.ETagHolder;
                this.FootMatts = vehicle.FootMatts == null ? false : (bool)vehicle.FootMatts;

                this.Details = vehicle.Details;
                this.Active = vehicle.IsActive == null ? true : (bool)vehicle.IsActive;

                this.vehicleIssues = vehicle.VehiclesIssues.Where(v => v.IsIssueFixed == false).ToList();
                this.vehicleImages = vehicle.VehiclesImages.ToList();
                this.PossessionsCount = vehicle.Possessions.Count;
                this.vehicleImages = vehicle.VehiclesImages.ToList();
                this.PossessionsCount = vehicle.Possessions.Count;
            }
        }

        public int VehicleID { get; set; }

        [Required]
        [Display(Name = "Rego")]
        [StringLength(7, ErrorMessage = "cannot be longer than 7 characters.")]
        public string RegistrationNo { get; set; }

        [Required]
        [Display(Name = "Rego Expiry Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> RegistrationExpiryDate { get; set; }
       
        [Display(Name = "Rego Expiry Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> RegoExpiryDate { get; set; }

        [Display(Name = "Make")]
        public string Make { get; set; }

        [Display(Name = "Model")]
        public string Model { get; set; }

        [Display(Name = "Make Year")]
        public Nullable<int> Year { get; set; }

        [Display(Name = "Colour")]
        public string Color { get; set; }

        [Display(Name = "First ODOMeter Reading")]
        public Nullable<float> FirstODOMeterReading { get; set; }

        [Display(Name = "ODO Meter Reading")]
        public Nullable<float> LastODOMeterReading { get; set; }

        [Display(Name = "Engine Service Due (KM)")]
        public Nullable<float> EngineServiceDueKM { get; set; }

        [Display(Name = "Transmission Service Due (KM)")]
        public Nullable<float> TransmissionServiceDueKM { get; set; }

        [Display(Name = "E-Tag Holder")]
        public bool ETagHolder { get; set; }

        [Display(Name = "Foot Matts")]
        public bool FootMatts { get; set; }

        [Display(Name = "Details")]
        public string Details { get; set; }

        public IList<VehiclesIssues> vehicleIssues;

        public IList<VehiclesImages> vehicleImages;

        [Display(Name = "Total Possessions")]
        public int PossessionsCount { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}