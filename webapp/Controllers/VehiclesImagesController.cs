using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VPS.Models;

namespace VPS.Controllers
{
    public class VehiclesImagesController : Controller
    {
        private VPSEntities db = new VPSEntities();

        // GET: /VehiclesImages/
        public ActionResult Index()
        {
            var vehiclesimages = db.VehiclesImages.Include(v => v.Vehicles);
            return View(vehiclesimages.ToList());
        }

        // GET: /VehiclesImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiclesImages vehiclesimages = db.VehiclesImages.Find(id);
            if (vehiclesimages == null)
            {
                return HttpNotFound();
            }
            return View(vehiclesimages);
        }
       
        public ActionResult Create(int? id)
        {
            VehiclesImages vImage = new VehiclesImages();
            vImage.VehicleID = id != null ? (int)id : 0;

            return View(vImage);
        }


        // POST: /VehiclesImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include="VehicleImageID,VehicleID,ImagePath")] VehiclesImages vehiclesimages)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.VehiclesImages.Add(vehiclesimages);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "RegistrationNo", vehiclesimages.VehicleID);
        //    return View(vehiclesimages);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form, HttpPostedFileBase file)
        {
            VehiclesImages dbVehicleImage = new VehiclesImages();

            if (ModelState.IsValid)
            {
                string name = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string extension = Path.GetExtension(file.FileName);
                dbVehicleImage.VehicleID = Convert.ToInt32(form["vehicleId"]);                                

                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + name + extension);
                    dbVehicleImage.ImagePath = "/Images/" + name + extension;
                }

                db.VehiclesImages.Add(dbVehicleImage);
                db.SaveChanges();
            }

            return RedirectToAction("Index", new { id = dbVehicleImage.VehicleID });
        }

        // GET: /VehiclesImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiclesImages vehiclesimages = db.VehiclesImages.Find(id);
            if (vehiclesimages == null)
            {
                return HttpNotFound();
            }
            return View(vehiclesimages);
        }

        // POST: /VehiclesImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehiclesImages vehiclesimages = db.VehiclesImages.Find(id);
            db.VehiclesImages.Remove(vehiclesimages);
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
