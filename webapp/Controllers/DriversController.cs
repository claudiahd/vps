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

namespace VPS.Controllers
{
	 [Authorize]
    public class DriversController : Controller
    {
        private VPSEntities db = new VPSEntities();

        // GET: Drivers
        public ActionResult Index()
        {
            List<MyDriver> myDrivers = new List<MyDriver>();

            foreach (var driver in db.Drivers.ToList())
            {
                MyDriver d = new MyDriver(driver);
                myDrivers.Add(d);
            }
            return View(myDrivers);
        }

        // GET: Drivers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drivers dbdrivers = db.Drivers.Find(id);
            if (dbdrivers == null)
            {
                return HttpNotFound();
            }
            MyDriver myDriver = new MyDriver(dbdrivers);                      
            return View(myDriver);
        }

        // GET: Drivers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DriverID,GivenName,MiddleName,SurName,DateOfBirth,Address,Suburb,State,Postcode,LicenceNo,MobileNo,EmergencyContactName,EmergencyContactRelation,EmergencyContactMobileNo,EmergencyContactAddress,UberID,UberIDPassword,EmailAddress")] MyDriver myDriver)
        {
            string from = Request.QueryString["from"];
            if (ModelState.IsValid)
            {
                Drivers dbDrivers = new Drivers();
                dbDrivers.GivenName = myDriver.GivenName;
                dbDrivers.MiddleName = myDriver.MiddleName;
                dbDrivers.SurName = myDriver.SurName;
                dbDrivers.DateOfBirth = myDriver.DateOfBirth;
                dbDrivers.Address = myDriver.Address;
                dbDrivers.Suburb = myDriver.Suburb;
                dbDrivers.State = myDriver.State;
                dbDrivers.Postcode = myDriver.Postcode;
                dbDrivers.MobileNo = myDriver.MobileNo;
                dbDrivers.LicenceNo = myDriver.LicenceNo;
                dbDrivers.UberID = myDriver.UberID;
                dbDrivers.UberIDPassword = myDriver.UberIDPassword;
                dbDrivers.EmailAddress = myDriver.EmailAddress;
                dbDrivers.EmergencyContactName = myDriver.EmergencyContactName;
                dbDrivers.EmergencyContactRelation = myDriver.EmergencyContactRelation;
                dbDrivers.EmergencyContactMobileNo = myDriver.EmergencyContactMobileNo;
                dbDrivers.EmergencyContactAddress = myDriver.EmergencyContactAddress;
                dbDrivers.IsActive = true;

                db.Drivers.Add(dbDrivers);
                db.SaveChanges();
                if (from == "possessionFlow")
                {
                    TempData["FromPossession"] = from;
                    return RedirectToAction("Create", "Possessions");
                }
                else
                {
                    return RedirectToAction("Index");
                }
                
            }

            return View(myDriver);
        }

        // GET: Drivers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drivers drivers = db.Drivers.Find(id);
            if (drivers == null)
            {
                return HttpNotFound();
            }

            MyDriver myDriver = new MyDriver(drivers);
            return View(myDriver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DriverID,GivenName,MiddleName,SurName,DateOfBirth,Address,Suburb,State,Postcode,LicenceNo,MobileNo,EmergencyContactName,EmergencyContactRelation,EmergencyContactMobileNo,EmergencyContactAddress,UberID,UberIDPassword,EmailAddress,IsActive")] Drivers drivers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(drivers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(drivers);
        }

        // GET: Drivers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drivers driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            MyDriver mydriver = new MyDriver(driver);
            return View(mydriver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Drivers drivers = db.Drivers.Find(id);
            if(drivers.Possessions.Count >0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.Drivers.Remove(drivers);
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
