using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace VPS.Models
{
    public class HomeModle
    {
        public IList<V_CurrentPossessions> CurrentPossessions { get; set; }
        public IList<V_AvailableVehicles> AvailableVehicles { get; set; }
        public IList<V_NoticedVehicles> NoticedVehicles { get; set; }

    }
}