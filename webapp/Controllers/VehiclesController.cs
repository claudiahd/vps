using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VPS.Models;
using VPS.Models.DTOs;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace VPS.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private VPSEntities db = new VPSEntities();

        // GET: Vehicles
        public async Task<ActionResult> Index()
        {
            List<VehicleDto> myVehicles = new List<VehicleDto>();
            List<Vehicles> vehicles = await db.Vehicles.OrderBy(v => v.RegistrationNo).ToListAsync();
            foreach (var vehicle in vehicles)
            {
                VehicleDto v = new VehicleDto(vehicle);
                myVehicles.Add(v);
            }
            return View(myVehicles);
        }

        // GET: Vehicles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicles dbVehicles = await db.Vehicles.FindAsync(id);
            if (dbVehicles == null)
            {
                return HttpNotFound();
            }
            VehicleDto myVehicle = new VehicleDto(dbVehicles);
            return View(myVehicle);
        }

        // GET: Vehicles/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "VehicleID,RegistrationNo,RegistrationExpiryDate,Make,Model,Year,Color,FirstODOMeterReading,LastODOMeterReading,EngineServiceDueKM,TransmissionServiceDueKM")] VehicleDto myVehicle)
        {
            if (ModelState.IsValid)
            {
                Vehicles dbVehicle = new Vehicles();
                dbVehicle.RegistrationNo = myVehicle.RegistrationNo;
                dbVehicle.RegistrationExpiryDate = myVehicle.RegistrationExpiryDate;
                dbVehicle.Make = myVehicle.Make;
                dbVehicle.Model = myVehicle.Model;
                dbVehicle.Year = myVehicle.Year;
                dbVehicle.Color = myVehicle.Color;
                dbVehicle.FirstODOMeterReading = myVehicle.FirstODOMeterReading;
                dbVehicle.LastODOMeterReading = myVehicle.LastODOMeterReading;
                dbVehicle.TransmissionServiceDueKM = myVehicle.TransmissionServiceDueKM;
                dbVehicle.EngineServiceDueKM = myVehicle.EngineServiceDueKM;
                dbVehicle.IsActive = true;
                //dbVehicle.ETagHolder = myVehicle.ETagHolder;
                //dbVehicle.FootMatts = myVehicle.FootMatts;



                db.Vehicles.Add(dbVehicle);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(myVehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicles vehicles = await db.Vehicles.FindAsync(id);
            if (vehicles == null)
            {
                return HttpNotFound();
            }

            return View(new VehicleDto(vehicles));
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "VehicleID,RegistrationNo,RegistrationExpiryDate,Make,Model,Year,Color,FirstODOMeterReading,LastODOMeterReading,EngineServiceDueKM,TransmissionServiceDueKM,Active")] VehicleDto myVehicles)
        {
            if (ModelState.IsValid)
            {
                Vehicles dbVehicle = await db.Vehicles.FindAsync(myVehicles.VehicleID);
                if (dbVehicle != null)
                {
                    dbVehicle.Color = myVehicles.Color;
                    dbVehicle.EngineServiceDueKM = myVehicles.EngineServiceDueKM;
                    dbVehicle.FirstODOMeterReading = myVehicles.FirstODOMeterReading;
                    dbVehicle.LastODOMeterReading = myVehicles.LastODOMeterReading;
                    dbVehicle.Make = myVehicles.Make;
                    dbVehicle.Model = myVehicles.Model;
                    dbVehicle.RegistrationExpiryDate = myVehicles.RegistrationExpiryDate;
                    dbVehicle.RegistrationNo = myVehicles.RegistrationNo;
                    dbVehicle.TransmissionServiceDueKM = myVehicles.TransmissionServiceDueKM;
                    dbVehicle.Year = myVehicles.Year;
                    dbVehicle.IsActive = myVehicles.Active;
                    //dbVehicles.ETagHolder = myVehicles.ETagHolder;
                    //dbVehicles.FootMatts = myVehicles.FootMatts;
                    db.Entry(dbVehicle).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(myVehicles);
        }

        // GET: Vehicles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicles dbVehicle = await db.Vehicles.FindAsync(id);
            if (dbVehicle == null)
            {
                return HttpNotFound();
            }
            VehicleDto myVehicle = new VehicleDto(dbVehicle);
            return View(myVehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            Vehicles vehicles = await db.Vehicles.FindAsync(id);


            if (vehicles.Possessions.Count > 0)
            {
                VehicleDto vehicle = new VehicleDto(vehicles);
                ViewBag.Error = "Vehicle is being used in possession and can't delete";
                return View(vehicles);
            }


            db.Vehicles.Remove(vehicles);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
