#region Using

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VPS.Models;
using System.Security.Cryptography;

#endregion

namespace VPS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        VPSEntities context = new VPSEntities();

        // GET: home/index
        public ActionResult Index()
        {
            HomeModle modle = new HomeModle();
            modle.CurrentPossessions = context.V_CurrentPossessions.OrderByDescending(v => v.PossessionDateTime).ToList();
            modle.AvailableVehicles = context.V_AvailableVehicles.ToList();
            modle.NoticedVehicles = context.V_NoticedVehicles.OrderBy(v => v.NoticeDate).ToList();

            return View(modle);
        }
    }
}