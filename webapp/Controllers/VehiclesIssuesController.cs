using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VPS.Models;

namespace VPS2.Controllers
{
    public class VehiclesIssuesController : Controller
    {
        private VPSEntities db = new VPSEntities();

        // GET: VehiclesIssues
        public async Task<ActionResult> Index(int? id)
        {
            Vehicles dbVehicles =await db.Vehicles.FindAsync(id);
            if (dbVehicles == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(dbVehicles.VehiclesIssues.OrderBy(vi => vi.IsIssueFixed).ToList());
        }

        // GET: VehiclesIssues/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiclesIssues vehiclesIssues = await db.VehiclesIssues.FindAsync(id);
            if (vehiclesIssues == null)
            {
                return HttpNotFound();
            }
            return View(vehiclesIssues);
        }

        // GET: VehiclesIssues/Create
        public ActionResult Create(int? id)
        {
            VehiclesIssues VehiclesIssues = new VehiclesIssues();
            VehiclesIssues.VehicleID = id != null ? (int)id : 0;

            return View(VehiclesIssues);
        }

        // POST: VehiclesIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<ActionResult> Create(FormCollection form, HttpPostedFileBase file)
        {
            VehiclesIssues dbVehicleIssues = new VehiclesIssues();

            if (ModelState.IsValid)
            {
                string name = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string extension = Path.GetExtension(file.FileName);
                dbVehicleIssues.VehicleID = Convert.ToInt32(form["vehicleId"]);
                dbVehicleIssues.IssueDetails = form["IssueDetails"];
                dbVehicleIssues.IssueReportDate = DateTime.Now;

                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + name + extension);
                    dbVehicleIssues.ImagePath = "/Images/" + name + extension;
                }

                db.VehiclesIssues.Add(dbVehicleIssues);
                await db.SaveChangesAsync();

            }

            return RedirectToAction("Index", new { id = dbVehicleIssues.VehicleID });
        }

        // GET: VehiclesIssues/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiclesIssues vehiclesIssues =await db.VehiclesIssues.FindAsync(id);
            if (vehiclesIssues == null)
            {
                return HttpNotFound();
            }
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "RegistrationNo", vehiclesIssues.VehicleID);
            return View(vehiclesIssues);
        }

        // POST: VehiclesIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "VehicleIssueID,VehicleID,IssueDetails,ImagePath,ImageThumbnailPath,IssueReportDate,IsIssueFixed,IssueFixeDate")] VehiclesIssues vehiclesIssues)
        {
            if (ModelState.IsValid)
            {
                VehiclesIssues dbVehiclesIssues = await db.VehiclesIssues.FindAsync(vehiclesIssues.VehicleIssueID);
                dbVehiclesIssues.IssueDetails = vehiclesIssues.IssueDetails;
                dbVehiclesIssues.IsIssueFixed = vehiclesIssues.IsIssueFixed;
                dbVehiclesIssues.IssueFixeDate = vehiclesIssues.IssueFixeDate;
                db.Entry(dbVehiclesIssues).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { id = vehiclesIssues.VehicleID });
            }
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "RegistrationNo", vehiclesIssues.VehicleID);
            return View(vehiclesIssues);
        }

        // GET: VehiclesIssues/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiclesIssues vehiclesIssues = await db.VehiclesIssues.FindAsync(id);
            if (vehiclesIssues == null)
            {
                return HttpNotFound();
            }
            return View(vehiclesIssues);
        }

        // POST: VehiclesIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VehiclesIssues vehiclesIssues =await db.VehiclesIssues.FindAsync(id);
            db.VehiclesIssues.Remove(vehiclesIssues);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { id = vehiclesIssues.VehicleID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
