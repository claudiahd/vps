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
            List<MyVehicle> myVehicles = new List<MyVehicle>();
            List<Vehicles> vehicles = await db.Vehicles.OrderBy(v => v.RegistrationNo).ToListAsync();
            foreach (var vehicle in vehicles)
            {
                MyVehicle v = new MyVehicle(vehicle);
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
<<<<<<< .mine
        public async Task<ActionResult> Create([Bind(Include = "VehicleID,RegistrationNo,RegistrationExpiryDate,Make,Model,Year,Color,FirstODOMeterReading,LastODOMeterReading,EngineServiceDueKM,TransmissionServiceDueKM")] MyVehicle myVehicle)
||||||| .r55
        public ActionResult Create([Bind(Include = "VehicleID,RegistrationNo,RegistrationExpiryDate,Make,Model,Year,Color,FirstODOMeterReading,LastODOMeterReading,EngineServiceDueKM,TransmissionServiceDueKM,ETagHolder,FootMatts")] MyVehicle myVehicle)
=======
        public ActionResult Create([Bind(Include = "VehicleID,RegistrationNo,RegistrationExpiryDate,Make,Model,Year,Color,FirstODOMeterReading,LastODOMeterReading,EngineServiceDueKM,TransmissionServiceDueKM")] MyVehicle myVehicle)
>>>>>>> .r70
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
<<<<<<< .mine
                dbVehicle.IsActive = true;
                //dbVehicle.ETagHolder = myVehicle.ETagHolder;
                //dbVehicle.FootMatts = myVehicle.FootMatts;
||||||| .r55
                dbVehicle.ETagHolder = myVehicle.ETagHolder;
                dbVehicle.FootMatts = myVehicle.FootMatts;
=======
                //dbVehicle.ETagHolder = myVehicle.ETagHolder;
                //dbVehicle.FootMatts = myVehicle.FootMatts;
>>>>>>> .r70



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

            return View(new MyVehicle(vehicles));
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
<<<<<<< .mine
        public async Task<ActionResult> Edit([Bind(Include = "VehicleID,RegistrationNo,RegistrationExpiryDate,Make,Model,Year,Color,FirstODOMeterReading,LastODOMeterReading,EngineServiceDueKM,TransmissionServiceDueKM,Active")] MyVehicle myVehicles)
||||||| .r55
        public ActionResult Edit([Bind(Include = "VehicleID,RegistrationNo,RegistrationExpiryDate,Make,Model,Year,Color,FirstODOMeterReading,LastODOMeterReading,EngineServiceDueKM,TransmissionServiceDueKM,ETagHolder,FootMatts")] MyVehicle myVehicles)
=======
        public ActionResult Edit([Bind(Include = "VehicleID,RegistrationNo,RegistrationExpiryDate,Make,Model,Year,Color,FirstODOMeterReading,LastODOMeterReading,EngineServiceDueKM,TransmissionServiceDueKM")] MyVehicle myVehicles)
>>>>>>> .r70
        {
            if (ModelState.IsValid)
            {
                Vehicles dbVehicles = await db.Vehicles.FindAsync(myVehicles.VehicleID);
                dbVehicles.Color = myVehicles.Color;
<<<<<<< .mine
                dbVehicles.EngineServiceDueKM = myVehicles.EngineServiceDueKM;
                dbVehicles.FirstODOMeterReading = myVehicles.FirstODOMeterReading;
||||||| .r55
                dbVehicles.EngineServiceDueKM = myVehicles.EngineServiceDueKM;
                dbVehicles.ETagHolder = myVehicles.ETagHolder;
                dbVehicles.FirstODOMeterReading = myVehicles.FirstODOMeterReading;
                dbVehicles.FootMatts = myVehicles.FootMatts;
=======
                dbVehicles.EngineServiceDueKM = myVehicles.EngineServiceDueKM;                
                dbVehicles.FirstODOMeterReading = myVehicles.FirstODOMeterReading;                
>>>>>>> .r70
                dbVehicles.LastODOMeterReading = myVehicles.LastODOMeterReading;
                dbVehicles.Make = myVehicles.Make;
                dbVehicles.Model = myVehicles.Model;
                dbVehicles.RegistrationExpiryDate = myVehicles.RegistrationExpiryDate;
                dbVehicles.RegistrationNo = myVehicles.RegistrationNo;
                dbVehicles.TransmissionServiceDueKM = myVehicles.TransmissionServiceDueKM;
                dbVehicles.Year = myVehicles.Year;
<<<<<<< .mine
                dbVehicles.IsActive = myVehicles.Active;
                //dbVehicles.ETagHolder = myVehicles.ETagHolder;
                //dbVehicles.FootMatts = myVehicles.FootMatts;
||||||| .r55

=======
                //dbVehicles.ETagHolder = myVehicles.ETagHolder;
                //dbVehicles.FootMatts = myVehicles.FootMatts;
>>>>>>> .r70
                db.Entry(dbVehicles).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
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
            MyVehicle myVehicle = new MyVehicle(dbVehicle);
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
                MyVehicle vehicle = new MyVehicle(vehicles);
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
