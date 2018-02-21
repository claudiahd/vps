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
using System.Threading.Tasks;

#endregion

namespace VPS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        VPSEntities context = new VPSEntities();

        // GET: home/index
        public async Task<ActionResult> Index()
        {
            HomeModle modle = new HomeModle();
            modle.CurrentPossessions = await context.V_CurrentPossessions.OrderByDescending(v => v.PossessionDateTime).ToListAsync();
            modle.AvailableVehicles = await context.V_AvailableVehicles.ToListAsync();
            modle.NoticedVehicles = await context.V_NoticedVehicles.OrderBy(v => v.NoticeDate).ToListAsync();

            return View(modle);
        }
    }
}