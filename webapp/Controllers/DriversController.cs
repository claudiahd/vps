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
using VPS.Models.DTOs;

namespace VPS.Controllers
{
    [Authorize]
    public class DriversController : Controller
    {
        private VPSEntities db = new VPSEntities();

        // GET: Drivers
        public async Task<ActionResult> Index()
        {
            List<DriverDto> dtoDrivers = new List<DriverDto>();

            foreach (var driver in await db.Drivers.ToListAsync())
            {
                DriverDto d = new DriverDto(driver);
                dtoDrivers.Add(d);
            }
            return View(dtoDrivers);
        }

        // GET: Drivers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drivers dbdrivers = await db.Drivers.FindAsync(id);
            if (dbdrivers == null)
            {
                return HttpNotFound();
            }
            DriverDto myDriver = new DriverDto(dbdrivers);
            return View(myDriver);
        }

        // GET: Drivers/Create
        public ActionResult Create()
        {
            return View();
        }

        //// POST: Drivers/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(FormCollection form, HttpPostedFileBase file)
        //{
        //    Drivers dbDrivers = new Drivers();
        //    string from = Request.QueryString["from"];
        //    if (ModelState.IsValid)
        //    {
                
        //        dbDrivers.GivenName = form["GivenName"];
        //        dbDrivers.MiddleName = form["MiddleName"];
        //        dbDrivers.SurName = form["SurName"];
        //        dbDrivers.DateOfBirth = DateTime.Parse(form["DateOfBirth"]);
        //        dbDrivers.Address = form["Address"];
        //        dbDrivers.Suburb = form["Suburb"];
        //        dbDrivers.State = form["State"];
        //        dbDrivers.Postcode = int.Parse(form["Postcode"]);
        //        dbDrivers.MobileNo = form["MobileNo"];
        //        dbDrivers.LicenceNo = form["LicenceNo"];
        //        dbDrivers.UberID = form["UberID"];
        //        dbDrivers.UberIDPassword = form["UberIDPassword"];
        //        dbDrivers.EmailAddress = form["EmailAddress"];
        //        dbDrivers.EmergencyContactName = form["EmergencyContactName"];
        //        dbDrivers.EmergencyContactRelation = form["EmergencyContactRelation"];
        //        dbDrivers.EmergencyContactMobileNo = form["EmergencyContactMobileNo"];
        //        dbDrivers.EmergencyContactAddress = form["EmergencyContactAddress"];
        //        //myDriver.AvatarPath
        //        dbDrivers.IsActive = true;

        //        db.Drivers.Add(dbDrivers);
        //        await db.SaveChangesAsync();
        //        if (from == "possessionFlow")
        //        {
        //            TempData["FromPossession"] = from;
        //            return RedirectToAction("Create", "Possessions");
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }

        //    return View(new MyDriver(dbDrivers));
        //}


        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
       // public async Task<ActionResult> Create([Bind(Include = "DriverID,GivenName,MiddleName,SurName,DateOfBirth,Address,Suburb,State,Postcode,LicenceNo,MobileNo,EmergencyContactName,EmergencyContactRelation,EmergencyContactMobileNo,EmergencyContactAddress,UberID,UberIDPassword,EmailAddress,AvatarPath")] MyDriver myDriver, HttpPostedFileBase file)
        public async Task<ActionResult> Create(DriverDto myDriver, HttpPostedFileBase file)
        {
            string from = Request.QueryString["from"];
            if (ModelState.IsValid)
            {
                if(db.Drivers.FirstOrDefaultAsync(d=> d.LicenceNo == myDriver.LicenceNo && d.DateOfBirth == myDriver.DateOfBirth) !=null)
                {
                    ModelState.AddModelError("LicenceNo", "Driver with same Licence Number or Date of Birth exist");
                    return View(myDriver);
                }

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
               
               
                if (file != null)
                {
                    string name = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string extension = Path.GetExtension(file.FileName);
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/Drivers/") + name + extension);
                    dbDrivers.AvatarPath = "/Images/Drivers/" + name + extension;
                }
                              
                dbDrivers.IsActive = true;

                db.Drivers.Add(dbDrivers);
                await db.SaveChangesAsync();
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
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drivers drivers = await db.Drivers.FindAsync(id);
            if (drivers == null)
            {
                return HttpNotFound();
            }

            DriverDto myDriver = new DriverDto(drivers);
            return View(myDriver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DriverDto myDriver, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                Drivers dbDriver = await db.Drivers.FindAsync(myDriver.DriverID);
                if (dbDriver != null)
                {
                    dbDriver.GivenName = myDriver.GivenName;
                    dbDriver.MiddleName = myDriver.MiddleName;
                    dbDriver.SurName = myDriver.SurName;
                    dbDriver.DateOfBirth = myDriver.DateOfBirth;
                    dbDriver.Address = myDriver.Address;
                    dbDriver.Suburb = myDriver.Suburb;
                    dbDriver.State = myDriver.State;
                    dbDriver.Postcode = myDriver.Postcode;
                    dbDriver.MobileNo = myDriver.MobileNo;
                    dbDriver.LicenceNo = myDriver.LicenceNo;
                    dbDriver.UberID = myDriver.UberID;
                    dbDriver.UberIDPassword = myDriver.UberIDPassword;
                    dbDriver.EmailAddress = myDriver.EmailAddress;
                    dbDriver.EmergencyContactName = myDriver.EmergencyContactName;
                    dbDriver.EmergencyContactRelation = myDriver.EmergencyContactRelation;
                    dbDriver.EmergencyContactMobileNo = myDriver.EmergencyContactMobileNo;
                    dbDriver.EmergencyContactAddress = myDriver.EmergencyContactAddress;
                    dbDriver.IsActive = myDriver.IsActive;

                   
                    if (file != null)
                    {
                        string name = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        string extension = Path.GetExtension(file.FileName);
                        file.SaveAs(HttpContext.Server.MapPath("~/Images/Drivers/") + name + extension);
                        dbDriver.AvatarPath = "/Images/Drivers/" + name + extension;
                    }

                    db.Entry(dbDriver).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
            return View(myDriver);
        }

        // GET: Drivers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drivers driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            DriverDto mydriver = new DriverDto(driver);
            return View(mydriver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Drivers drivers = db.Drivers.Find(id);
            if (drivers.Possessions.Count > 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.Drivers.Remove(drivers);
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
