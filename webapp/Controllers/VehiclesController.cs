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

namespace VPS.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private VPSEntities db = new VPSEntities();

        // GET: Vehicles
        public ActionResult Index()
        {
            List<MyVehicle> myVehicles = new List<MyVehicle>();

            foreach (var vehicle in db.Vehicles.OrderBy(v => v.RegistrationNo).ToList())
            {
                MyVehicle v = new MyVehicle(vehicle);
                myVehicles.Add(v);
            }

            return View(myVehicles);

        }

        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicles dbVehicles = db.Vehicles.Find(id);
            if (dbVehicles == null)
            {
                return HttpNotFound();
            }
            MyVehicle myVehicle = new MyVehicle(dbVehicles);
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
        public ActionResult Create([Bind(Include = "VehicleID,RegistrationNo,RegistrationExpiryDate,Make,Model,Year,Color,FirstODOMeterReading,LastODOMeterReading,EngineServiceDueKM,TransmissionServiceDueKM")] MyVehicle myVehicle)
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
                //dbVehicle.ETagHolder = myVehicle.ETagHolder;
                //dbVehicle.FootMatts = myVehicle.FootMatts;



                db.Vehicles.Add(dbVehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(myVehicle);
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicles vehicles = db.Vehicles.Find(id);
            if (vehicles == null)
            {
                return HttpNotFound();
            }

            return View(new MyVehicle(vehicles));
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VehicleID,RegistrationNo,RegistrationExpiryDate,Make,Model,Year,Color,FirstODOMeterReading,LastODOMeterReading,EngineServiceDueKM,TransmissionServiceDueKM")] MyVehicle myVehicles)
        {
            if (ModelState.IsValid)
            {
                Vehicles dbVehicles = db.Vehicles.Find(myVehicles.VehicleID);
                dbVehicles.Color = myVehicles.Color;
                dbVehicles.EngineServiceDueKM = myVehicles.EngineServiceDueKM;                
                dbVehicles.FirstODOMeterReading = myVehicles.FirstODOMeterReading;                
                dbVehicles.LastODOMeterReading = myVehicles.LastODOMeterReading;
                dbVehicles.Make = myVehicles.Make;
                dbVehicles.Model = myVehicles.Model;
                dbVehicles.RegistrationExpiryDate = myVehicles.RegistrationExpiryDate;
                dbVehicles.RegistrationNo = myVehicles.RegistrationNo;
                dbVehicles.TransmissionServiceDueKM = myVehicles.TransmissionServiceDueKM;
                dbVehicles.Year = myVehicles.Year;
                //dbVehicles.ETagHolder = myVehicles.ETagHolder;
                //dbVehicles.FootMatts = myVehicles.FootMatts;
                db.Entry(dbVehicles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(myVehicles);
        }

        // GET: Vehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicles dbVehicle = db.Vehicles.Find(id);
            if (dbVehicle == null)
            {
                return HttpNotFound();
            }
            MyVehicle myVehicle = new MyVehicle(dbVehicle);
            return View(myVehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Vehicles vehicles = db.Vehicles.Find(id);


            if (vehicles.Possessions.Count > 0)
            {
                MyVehicle vehicle = new MyVehicle(vehicles);
                ViewBag.Error = "Vehicle is being used in possession and can't delete";
                return View(vehicles);
            }


            db.Vehicles.Remove(vehicles);
            db.SaveChanges();
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
