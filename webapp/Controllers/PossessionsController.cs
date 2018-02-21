using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VPS.Models;
using VPS.Models;
using VPS.Models.DTOs;
using ConsumedByCode.SignatureToImage;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Threading.Tasks;

namespace VPS.Controllers
{
    [Authorize]
    public class PossessionsController : Controller
    {
        private VPSEntities db = new VPSEntities();

        // GET: Possessions
        public async Task<ActionResult> Index()
        {
            var possessions = await db.Possessions.Include(p => p.Drivers).Include(p => p.Vehicles).Include(p => p.Users).Include(p => p.Users1).Include(p => p.PossessionsIssues).OrderByDescending(p => p.PossessionID).ToListAsync();
            List<PossessionDto> myPossessions = new List<PossessionDto>();

            foreach (var p in possessions)
            {
                myPossessions.Add(new PossessionDto(p));
            }

            return View(myPossessions);

        }

        // GET: Possessions
        public async Task<ActionResult> PossessionByVehicle(int? id)
        {
            var possessions = await db.Possessions.Include(p => p.Drivers).Include(p => p.Vehicles).Include(p => p.Users).Include(p => p.Users1).Include(p => p.PossessionsIssues).OrderByDescending(p => p.PossessionID).Where(p => p.VehicleID == id).ToListAsync();
            List<PossessionDto> myPossessions = new List<PossessionDto>();

            foreach (var p in possessions)
            {
                myPossessions.Add(new PossessionDto(p));
            }

            return View(myPossessions);

        }

        // GET: Possessions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Possessions possessions = await db.Possessions.FindAsync(id);
            if (possessions == null)
            {
                return HttpNotFound();
            }

            return View(new PossessionDto(possessions));
        }

        // GET: Possessions/ReturnPossession/5
        public async Task<ActionResult> ReturnPossession(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Possessions possessions = await db.Possessions.FindAsync(id);
            if (possessions == null)
            {
                return HttpNotFound();
            }
            PossessionDto myPossession = new PossessionDto(possessions);
            return View(myPossession);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReturnPossession(PossessionDto myPossessionReturn)
        {
            if (ModelState.IsValid)
            {
                Users user = await db.Users.FirstOrDefaultAsync();
                string userId = user != null ? user.UserID.ToString() : "1";

                if (Request.Cookies["LoginUserID"] != null)
                {
                    userId = Request.Cookies["LoginUserID"].Value;
                }

                if (myPossessionReturn.PossessionID < 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Possessions possessions = await db.Possessions.FindAsync(myPossessionReturn.PossessionID);
                if (possessions == null)
                {
                    return HttpNotFound();
                }


                DateTime date = myPossessionReturn.ReturnDate.Value;

                DateTime time;
                if (DateTime.TryParse(myPossessionReturn.ReturnTime, out time))
                {
                    date = date.AddHours(time.Hour);
                    date = date.AddMinutes(time.Minute);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Time");
                    return View();
                }

                possessions.ReturnDateTime = date;

                possessions.IsReturn = possessions.Vehicles.IsAvailable = true;
                possessions.Vehicles.LastODOMeterReading =
                    possessions.ODOMeterReturn = myPossessionReturn.ODOMeterReturn;
                possessions.Vehicles.EngineServiceDueKM = myPossessionReturn.ServiceDue;
                possessions.ETagHolderReturn = myPossessionReturn.ETagHolderReturn;
                possessions.FootMattsReturn = myPossessionReturn.FootMattsReturn;
                possessions.CleanlinessReturn = myPossessionReturn.CleanlinessReturn;
                //possessions.EngineServiceDueReturn = myPossessionReturn.EngineServiceDueReturn;
                //possessions.TransmissionServiceDueReturn = myPossessionReturn.TransmissionServiceDueReturn;
                possessions.Feedback = myPossessionReturn.Feedback;

                possessions.ReturnByUserID = userId != string.Empty ? Convert.ToInt32(userId) : 1;

                db.Entry(possessions).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // GET: Possessions/AddNotice/5
        public async Task<ActionResult> AddNotice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Possessions possessions = await db.Possessions.FindAsync(id);
            if (possessions == null)
            {
                return HttpNotFound();
            }
            PossessionDto myPossession = new PossessionDto(possessions);
            return View(myPossession);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNotice(PossessionDto myPossessionReturn)
        {

            if (myPossessionReturn.PossessionID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Possessions possessions = await db.Possessions.FindAsync(myPossessionReturn.PossessionID);
            if (possessions == null)
            {
                return HttpNotFound();
            }

            possessions.NoticeDateReturn = myPossessionReturn.NoticeDateReturn;
            possessions.NoticeRemarks = myPossessionReturn.NoticeRemarks;

            db.Entry(possessions).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Possessions/Create
        public async Task<ActionResult> Create(int? driverId, int? vehicleId)
        {
            string fromPossession = "";
            ViewBag.driverId = driverId;
            ViewBag.vehicleId = vehicleId;
            if (TempData["FromPossession"] != null)
            {
                fromPossession = TempData["FromPossession"].ToString();
            }

            if (fromPossession == "possessionFlow")
            {
                PossessionDto myPossession1 = (PossessionDto)TempData["PossessionData"];

                return View(myPossession1);
            }
            else
            {
                PossessionDto myPossession = new PossessionDto();

                var driverListFromDB = await db.Drivers.Where(d => d.IsActive == true).ToListAsync();
                var vehicleListFromDB = await db.V_AvailableVehicles.ToListAsync();

                DropDownList ddList = new DropDownList();
                ddList.key = -1;
                ddList.value = "Please Select Driver";
                myPossession.driverList.Add(ddList);
                foreach (var item in driverListFromDB)
                {
                    ddList = new DropDownList();
                    ddList.key = item.DriverID;
                    if (driverId == item.DriverID)
                    {
                        myPossession.selectedDriver = item.DriverID.ToString();
                    }
                    ddList.value = item.GivenName + " " + item.MiddleName + " " + item.SurName;
                    myPossession.driverList.Add(ddList);
                }

                ddList = new DropDownList();
                ddList.key = -1;
                ddList.value = "Please Select Vehicle";
                myPossession.vehicleList.Add(ddList);
                foreach (var item in vehicleListFromDB)
                {
                    ddList = new DropDownList();
                    ddList.key = item.VehicleID;
                    if (vehicleId == item.VehicleID)
                    {
                        myPossession.selectedVehicle = item.VehicleID.ToString();
                    }
                    ddList.value = item.RegistrationNo;
                    myPossession.vehicleList.Add(ddList);
                }
                //ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "GivenName");
                //ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "RegistrationNo");
                //ViewBag.PossessionByUserID = new SelectList(db.Users, "UserID", "Name");
                //ViewBag.ReturnByUserID = new SelectList(db.Users, "UserID", "Name");
                return View(myPossession);
            }
        }

        // POST: Possessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FormCollection form)
        {

            if (ModelState.IsValid)
            {
                string userId = db.Users.FirstOrDefault().UserID.ToString();
                if (Request.Cookies["LoginUserID"] != null)
                {
                    userId = Request.Cookies["LoginUserID"].Value;
                }

                Possessions dbPossesions = new Possessions();
                dbPossesions.DriverID = Convert.ToInt32(form["selectedDriver"].ToString());
                dbPossesions.Vehicles = await db.Vehicles.FindAsync(Convert.ToInt32(form["selectedVehicle"].ToString()));
                dbPossesions.RegistrationNo = form["vehicleReg"];


                DateTime date = DateTime.Parse(form["pDate"]);
                DateTime time = DateTime.Parse(form["pTime"]);
                date = date.AddHours(time.Hour);
                date = date.AddMinutes(time.Minute);
                dbPossesions.PossessionDateTime = date;

                dbPossesions.ODOMeterPossissionTime = (form["vehicleLastODOMeterReading"] != "") ? (float)Convert.ToDecimal(form["vehicleLastODOMeterReading"]) : 00;
                dbPossesions.ETagHolderPossissionTime = form["ETagHolder"] == "false" ? false : true;
                dbPossesions.FootMattsPossissionTime = form["FootMatts"] == "false" ? false : true;
                dbPossesions.CleanlinessPossissionTime = form["clean"] == "false" ? false : true;
                dbPossesions.EngineServiceDuePossissionTime = (form["vehicleEngineServiceDueKM"] != "") ? (float)Convert.ToDecimal(form["vehicleEngineServiceDueKM"]) : 00;
                dbPossesions.TransmissionServiceDuePossissionTime = null;
                dbPossesions.NoticeDateReturn = null;
                dbPossesions.NoticeRemarks = null;
                dbPossesions.ReturnDateTime = null;
                dbPossesions.ODOMeterReturn = null;
                dbPossesions.ETagHolderReturn = false;
                dbPossesions.FootMattsReturn = false;
                dbPossesions.CleanlinessReturn = false;
                dbPossesions.EngineServiceDueReturn = null;
                dbPossesions.TransmissionServiceDueReturn = null;
                dbPossesions.Feedback = null;
                dbPossesions.IsReturn = false;
                dbPossesions.PossessionByUserID = userId != string.Empty ? Convert.ToInt32(userId) : 1;
                dbPossesions.ReturnByUserID = null;

                dbPossesions.PossessionTimeDriverSignature = form["driver_signature"].Replace('\"', ' ');
                dbPossesions.PossessionTimeContractorSignature = form["contractor_signature"].Replace('\"', ' ');
                foreach (var vIssue in dbPossesions.Vehicles.VehiclesIssues)
                {
                    PossessionsIssues pTimeIssue = new PossessionsIssues();
                    pTimeIssue.ImagePath = vIssue.ImagePath;
                    pTimeIssue.IssueDetails = vIssue.IssueDetails;
                    pTimeIssue.IsPossessionTimeIssue = true;
                    dbPossesions.PossessionsIssues.Add(pTimeIssue);
                }

                foreach (var vImages in dbPossesions.Vehicles.VehiclesImages)
                {
                    PossessionsImages pTimeImg = new PossessionsImages();
                    pTimeImg.PossessionTimeImagePath = vImages.ImagePath;
                    pTimeImg.ReturnTimeImagePath = "";
                    dbPossesions.PossessionsImages.Add(pTimeImg);
                }

                if (Request.Form["addDriver"] != null)
                {
                    TempData["PossessionData"] = dbPossesions;
                    return RedirectToAction("Create", "Drivers", new { from = "possessionFlow" });
                }
                else if (Request.Form["Create"] != null)
                {
                    dbPossesions.Vehicles.IsAvailable = false;
                    db.Possessions.Add(dbPossesions);

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Index");
        }

        //// GET: Possessions/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Possessions possessions = db.Possessions.Find(id);
        //    if (possessions == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "GivenName", possessions.DriverID);
        //    ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "RegistrationNo", possessions.VehicleID);
        //    ViewBag.PossessionByUserID = new SelectList(db.Users, "UserID", "Name", possessions.PossessionByUserID);
        //    ViewBag.ReturnByUserID = new SelectList(db.Users, "UserID", "Name", possessions.ReturnByUserID);
        //    return View(possessions);
        //}

        //// POST: Possessions/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "PossessionID,DriverID,VehicleID,RegistrationNo,PossessionDate,ODOMeterPossissionTime,ETagHolderPossissionTime,FootMattsPossissionTime,CleanlinessPossissionTime,EngineServiceDuePossissionTime,TransmissionServiceDuePossissionTime,NoticeDateReturn,NoticeRemarks,ReturnDateTime,ODOMeterReturn,ETagHolderReturn,FootMattsReturn,CleanlinessReturn,EngineServiceDueReturn,TransmissionServiceDueReturn,Feedback,IsReturn,PossessionByUserID,ReturnByUserID")] Possessions possessions)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(possessions).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.DriverID = new SelectList(db.Drivers, "DriverID", "GivenName", possessions.DriverID);
        //    ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "RegistrationNo", possessions.VehicleID);
        //    ViewBag.PossessionByUserID = new SelectList(db.Users, "UserID", "Name", possessions.PossessionByUserID);
        //    ViewBag.ReturnByUserID = new SelectList(db.Users, "UserID", "Name", possessions.ReturnByUserID);
        //    return View(possessions);
        //}

        //// GET: Possessions/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Possessions possessions = db.Possessions.Find(id);
        //    if (possessions == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(possessions);
        //}

        //// POST: Possessions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Possessions possessions = db.Possessions.Find(id);
        //    db.Possessions.Remove(possessions);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public async Task<JsonResult> DriverInfo(int driverId)
        {
            if (driverId != -1)
            {
                TempData["driverId"] = driverId;
                var driverDetail = await db.Drivers.FirstOrDefaultAsync(x => x.DriverID == driverId);
                DriverDto myDriver = new DriverDto();
                myDriver.DriverID = driverId;
                myDriver.GivenName = driverDetail.GivenName;
                myDriver.SurName = driverDetail.SurName;
                myDriver.Address = driverDetail.Address + ", " + driverDetail.Suburb + ", " + driverDetail.State + " " + driverDetail.Postcode;
                myDriver.MobileNo = driverDetail.MobileNo;
                myDriver.LicenceNo = driverDetail.LicenceNo;

                return Json(myDriver, JsonRequestBehavior.AllowGet);
            }
            return Json(new DriverDto(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> VehicleInfo(int vehicleId)
        {
            if (vehicleId != -1)
            {
                TempData["vehicleId"] = vehicleId;
                var vehicleDetail = await db.Vehicles.FirstOrDefaultAsync(x => x.VehicleID == vehicleId);
                IList<VehiclesIssues> vehicleIssues = await db.VehiclesIssues.Where(x => x.VehicleID == vehicleId && x.IsIssueFixed == false).ToListAsync();
                IList<VehiclesImages> vehicleImages = await db.VehiclesImages.Where(x => x.VehicleID == vehicleId).ToListAsync();
                VehicleDto myVehicle = new VehicleDto();
                myVehicle.vehicleIssues = new List<VehiclesIssues>();
                foreach (var item in vehicleIssues)
                {
                    VehiclesIssues vi = new VehiclesIssues();
                    vi.IssueDetails = item.IssueDetails;
                    vi.ImagePath = item.ImagePath;
                    myVehicle.vehicleIssues.Add(vi);
                }

                myVehicle.vehicleImages = new List<VehiclesImages>();
                foreach (var item in vehicleImages)
                {
                    VehiclesImages vImg = new VehiclesImages();
                    vImg.ImagePath = item.ImagePath;
                    myVehicle.vehicleImages.Add(vImg);
                }

                myVehicle.RegistrationNo = vehicleDetail.RegistrationNo;
                myVehicle.Make = vehicleDetail.Make;
                myVehicle.Model = vehicleDetail.Model;
                myVehicle.Year = vehicleDetail.Year;
                myVehicle.Color = vehicleDetail.Color;
                myVehicle.FirstODOMeterReading = vehicleDetail.FirstODOMeterReading;
                myVehicle.LastODOMeterReading = vehicleDetail.LastODOMeterReading;
                myVehicle.EngineServiceDueKM = vehicleDetail.EngineServiceDueKM;
                myVehicle.ETagHolder = vehicleDetail.ETagHolder == null ? false : (bool)vehicleDetail.ETagHolder;
                myVehicle.FootMatts = vehicleDetail.FootMatts == null ? false : (bool)vehicleDetail.FootMatts;

                return Json(myVehicle, JsonRequestBehavior.AllowGet);
            }
            return Json(new VehicleDto(), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToRouteResult> CreateDriver([Bind(Include = "DriverID,GivenName,MiddleName,SurName,DateOfBirth,Address,Suburb,State,Postcode,LicenceNo,MobileNo,EmergencyContactName,EmergencyContactRelation,EmergencyContactMobileNo,EmergencyContactAddress,UberID,UberIDPassword,EmailAddress")] DriverDto myDriver)
        {
            string from = Request.QueryString["from"];
            int newDriverId = -1;
            if (ModelState.IsValid)
            {
                Drivers dbDrivers = new Drivers();
                dbDrivers = db.Drivers.Create();
                dbDrivers.DriverID = 0;
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

                db.Drivers.Add(dbDrivers);
                await db.SaveChangesAsync();
                newDriverId = dbDrivers.DriverID;
                TempData["DriverIdAdded"] = newDriverId;
                return RedirectToAction("Create", new { @driverId = newDriverId, @vehicleId = TempData["vehicleId"] });

            }
            return RedirectToAction("Create", new { @driverId = newDriverId, @vehicleId = ViewBag.vehicleId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToRouteResult> CreateVehicleIssue(FormCollection form, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                string name = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string extension = Path.GetExtension(file.FileName);
                VehiclesIssues dbVehicleIssues = new VehiclesIssues();
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + name + extension);
                    dbVehicleIssues.ImagePath = "/Images/" + name + extension;
                }

                dbVehicleIssues.VehicleID = Convert.ToInt32(TempData["vehicleId"]);
                dbVehicleIssues.IssueDetails = form["myVehicleIssue.IssueDetails"];
                dbVehicleIssues.ImageThumbnailPath = form["myVehicleIssue.ImageThumbnailPathform"];
                dbVehicleIssues.IssueReportDate = DateTime.Now; ;


                db.VehiclesIssues.Add(dbVehicleIssues);
                await db.SaveChangesAsync();

                return RedirectToAction("Create", new { @newDriverId = TempData["DriverIdAdded"], @vehicleId = TempData["vehicleId"], @driverId = TempData["driverId"] });

            }
            return RedirectToAction("Create", new { @newDriverId = TempData["DriverIdAdded"], @vehicleId = TempData["vehicleId"], @driverId = TempData["driverId"] });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToRouteResult> CreateVehicleImage(FormCollection form, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                VehiclesImages dbVehicleImage = new VehiclesImages();
                if (file != null)
                {
                    string name = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string extension = Path.GetExtension(file.FileName);

                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + name + extension);
                    dbVehicleImage.ImagePath = "/Images/" + name + extension;
                    dbVehicleImage.VehicleID = Convert.ToInt32(TempData["vehicleId"]);

                    db.VehiclesImages.Add(dbVehicleImage);
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Create", new { @newDriverId = TempData["DriverIdAdded"], @vehicleId = TempData["vehicleId"], @driverId = TempData["driverId"] });
        }
    }
}
