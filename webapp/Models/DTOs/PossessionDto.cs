using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace VPS.Models.DTOs
{

    public class PossessionDto
    {

        public PossessionDto()
        {

        }

        public PossessionDto(Possessions p)
        {
            PossessionID = p.PossessionID;
            DriverID = p.DriverID;
            VehicleID = p.VehicleID;
            RegistrationNo = p.RegistrationNo;
            PossessionDate = p.PossessionDateTime;
            PossessionTime = p.PossessionDateTime.ToString("HH:mm tt"); 
            ODOMeterPossissionTime = p.ODOMeterPossissionTime;
            ETagHolderPossissionTime = p.ETagHolderPossissionTime == null ? false : (bool)p.ETagHolderPossissionTime;
            FootMattsPossissionTime = p.FootMattsPossissionTime == null ? false : (bool)p.FootMattsPossissionTime;
            CleanlinessPossissionTime = p.CleanlinessPossissionTime == null ? false : (bool)p.CleanlinessPossissionTime;
            EngineServiceDuePossissionTime = p.EngineServiceDuePossissionTime == null ? 0 : (float)p.EngineServiceDuePossissionTime;
            TransmissionServiceDuePossissionTime = p.TransmissionServiceDuePossissionTime == null ? 0 : (float)p.TransmissionServiceDuePossissionTime;
            NoticeDateReturn = p.NoticeDateReturn;
            NoticeRemarks = p.NoticeRemarks;

            if (p.ReturnDateTime.HasValue)
            {
                ReturnDate = p.ReturnDateTime.Value.Date;
                ReturnTime = p.ReturnDateTime.Value.ToString("HH:mm tt");              
            }

            Drivers = p.Drivers;
            DriversName = p.Drivers.GivenName + " " + p.Drivers.MiddleName + " " + p.Drivers.SurName;
            Vehicles = p.Vehicles;
            PossessionByUser = p.Users;
            PossessionByName = p.Users.Name;
            PossessionResturnByUsers = p.Users1;
            ReturnByName = p.Users1 != null ? p.Users1.Name : "";
            IsReturn = p.IsReturn == null ? false : (bool)p.IsReturn;
            ODOMeterReturn = p.ODOMeterReturn;
            ETagHolderReturn = p.ETagHolderReturn == null ? false : (bool)p.ETagHolderReturn;
            FootMattsReturn = p.FootMattsReturn == null ? false : (bool)p.FootMattsReturn;
            CleanlinessReturn = p.CleanlinessReturn == null ? false : (bool)p.CleanlinessReturn;
            EngineServiceDueReturn = p.EngineServiceDueReturn == null ? false : (bool)p.EngineServiceDueReturn;
            TransmissionServiceDueReturn = p.TransmissionServiceDueReturn == null ? false : (bool)p.TransmissionServiceDueReturn;
            Feedback = p.Feedback;
            DriverSignature = p.PossessionTimeDriverSignature;
            ContractorSignature = p.PossessionTimeContractorSignature;
            PossessionsIssues = p.PossessionsIssues;
            PossessionsImages = p.PossessionsImages;
        }

        [Display(Name = "ID")]
        public int PossessionID { get; set; }

        public int DriverID { get; set; }

        public int VehicleID { get; set; }

        [Display(Name = "Rego")]
        public string RegistrationNo { get; set; }

        [Display(Name = "Possess Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public System.DateTime PossessionDate { get; set; }

        [Display(Name = "Possess Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{HH:mm tt}")]
        public string PossessionTime { get; set; }

        [Display(Name = "ODOMeter on Pos.")]
        public float ODOMeterPossissionTime { get; set; }

        [Display(Name = "E-Tag on Pos.")]
        public bool ETagHolderPossissionTime { get; set; }

        [Display(Name = "FootMatt on Pos.")]
        public bool FootMattsPossissionTime { get; set; }

        [Display(Name = "Cleanliness on Pos.")]
        public bool CleanlinessPossissionTime { get; set; }

        [Display(Name = "Eng. Servve Due on Pos.")]
        public float EngineServiceDuePossissionTime { get; set; }

        [Display(Name = "Servve Due on Pos.")]
        public float TransmissionServiceDuePossissionTime { get; set; }

        [Display(Name = "Notice Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> NoticeDateReturn { get; set; }

        [Display(Name = "Notice Remarks")]
        public string NoticeRemarks { get; set; }

        [Display(Name = "Return Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> ReturnDate { get; set; }

        [Display(Name = "Return Time")]
        public string ReturnTime { get; set; }

        [Display(Name = "ODOMetter on Return")]
        public Nullable<float> ODOMeterReturn { get; set; }

        [Display(Name = "ETag on Return")]
        public bool ETagHolderReturn { get; set; }

        [Display(Name = "FootMatts on Return")]
        public bool FootMattsReturn { get; set; }

        [Display(Name = "Cleanliness on Return")]
        public bool CleanlinessReturn { get; set; }

        [Display(Name = "Eng. Service Due Return Time")]
        public bool EngineServiceDueReturn { get; set; }

        [Display(Name = "Transmission Srv Due Returen Time")]
        public bool TransmissionServiceDueReturn { get; set; }

        [Display(Name = "Feedback")]
        public string Feedback { get; set; }

        [Display(Name = "Return")]
        public bool IsReturn { get; set; }

        [Display(Name = "Possession By")]
        public Nullable<int> PossessionByUserID { get; set; }

        [Display(Name = "Possess By")]
        public string PossessionByName { get; set; }

        [Display(Name = "Driver Name")]
        public string DriversName { get; set; }

        [Display(Name = "Return By")]
        public Nullable<int> ReturnByUserID { get; set; }

        [Display(Name = "Return By")]
        public string ReturnByName { get; set; }

        [Display(Name = "Driver Signature")]
        public string DriverSignature { get; set; }

        [Display(Name = "Contractor Signature")]
        public string ContractorSignature { get; set; }

        // this is only to enter service due at the time of return
        [Display(Name = "Engine Service Due (KM)")]
        public float ServiceDue { get; set; }


        public virtual Drivers Drivers { get; set; }
        public virtual Vehicles Vehicles { get; set; }
        public virtual Users PossessionByUser { get; set; }
        public virtual ICollection<PossessionsImages> PossessionsImages { get; set; }
        public virtual ICollection<PossessionsIssues> PossessionsIssues { get; set; }
        public virtual Users PossessionResturnByUsers { get; set; }
        public List<DropDownList> driverList = new List<DropDownList>();
        public string selectedDriver { get; set; }
        public List<DropDownList> vehicleList = new List<DropDownList>();
        public string selectedVehicle { get; set; }
        public DriverDto myDriver { get; set; }

        public VehicleIssueDto myVehicleIssue { get; set; }
    }

    public class DropDownList
    {
        public int key { get; set; }
        public string value { get; set; }
    }

}