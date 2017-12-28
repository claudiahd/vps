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


namespace VPS.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private VPSEntities db = new VPSEntities();

        // GET: Users

        public ActionResult Index()
        {
            List<MyUser> myUsers = new List<MyUser>();

            foreach (var user in db.Users.OrderBy(u => u.UserTypeID).ToList())
            {
                MyUser u = new MyUser(user);
                myUsers.Add(u);
            }

            return View(myUsers);
        }

        // GET: Users/Details/5                
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users dbUser = db.Users.Find(id);
            if (dbUser == null)
            {
                return HttpNotFound();
            }

            MyUser myUser = new MyUser(dbUser);

            return View(myUser);
        }

        // GET: Users/Create        
        public ActionResult Create()
        {
            ViewBag.UserTypeID = GetUSerTypes();

            return View();
        }

        private List<SelectListItem> GetUSerTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in Helper.GetValues<UserTypes>())
            {
                items.Add(new SelectListItem { Text = item.ToString(), Value = ((int)item).ToString() });
            }

            return items;
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Name,EmailAddress,ReEmailAddress,Password,VerifyPassword,UserTypeID,Active")] MyUser myUser)
        {
            if (ModelState.IsValid)
            {
                Users dbUser = new Users();

                dbUser.Name = myUser.Name;
                dbUser.EmailAddress = myUser.EmailAddress;
                using (MD5 md5Hash = MD5.Create())
                {
                    dbUser.Password = Helper.GetMd5Hash(md5Hash, myUser.Password);
                }
                dbUser.Active = myUser.Active;
                dbUser.UserTypeID = myUser.UserTypeID;

                db.Users.Add(dbUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.UserTypeID = GetUSerTypes();
            }

            return View(myUser);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in Helper.GetValues<UserTypes>())
            {
                items.Add(new SelectListItem { Text = item.ToString(), Value = ((int)item).ToString(), Selected = ((int)item) == users.UserTypeID });
            }

            ViewBag.UserTypeID = items;

            MyUser myUser = new MyUser(users);
            return View(myUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Name,EmailAddress,Password,UserTypeID,Active")] MyUser user)
        {
            if (ModelState.IsValid)
            {


                if (user.UserID == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Users dbUser = db.Users.Find(user.UserID);

                if (dbUser == null)
                {
                    return HttpNotFound();
                }


                dbUser.Name = user.Name;
                dbUser.Active = user.Active;
                dbUser.EmailAddress = user.EmailAddress;
                dbUser.UserTypeID = user.UserTypeID;

                if (user.Password != "**********")
                {
                    using (MD5 md5Hash = MD5.Create())
                    {
                        dbUser.Password = Helper.GetMd5Hash(md5Hash, user.Password);
                    }
                }

                db.Entry(dbUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }

            MyUser user = new MyUser(users);

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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
