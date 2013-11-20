using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UltimateSecuritySurvey.Models;

namespace UltimateSecuritySurvey.Controllers
{
    public class UserController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View(db.UserAccounts.ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            UserAccount useraccount = db.UserAccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            return View(useraccount);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View("CreateEdit", new UserAccount());
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserAccount useraccount = db.UserAccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            
            return View("CreateEdit", useraccount);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(UserAccount useraccount)
        {
            if (ModelState.IsValid)
            {
                if (useraccount.userId <= 0)
                {
                    db.UserAccounts.Add(useraccount);
                }
                else
                {
                    db.Entry(useraccount).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(useraccount);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserAccount useraccount = db.UserAccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            return View(useraccount);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserAccount useraccount = db.UserAccounts.Find(id);
            db.UserAccounts.Remove(useraccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}