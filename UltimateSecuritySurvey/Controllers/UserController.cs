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
    /// <summary>
    /// Controller to display and manage users
    /// </summary>
    public class UserController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();
  
        /// <summary>
        /// Lists useraccounts on the index page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Error = TempData["Message"];
            return View(db.UserAccounts.ToList());
        }

        /// <summary>
        /// Finds useraccount with the id from the database for the details page
        /// </summary>
        /// <param name="id">Primary id for User</param>
        public ActionResult Details(int id = 0)
        {
            UserAccount useraccount = db.UserAccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            return View(useraccount);
        }

        /// <summary>
        /// Method to go to the Create new view
        /// </summary>
        public ActionResult Create()
        {
            return View("CreateEdit", new UserAccount());
        }

        /// <summary>
        /// Method to go to the edit view, also gets the id from the db
        /// </summary>
        /// <param name="useraccount">UserAccount class object</param>
        public ActionResult Edit(int id = 0)
        {
            UserAccount useraccount = db.UserAccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            
            return View("CreateEdit", useraccount);
        }

        /// <summary>
        /// Method to edit the user and save the changes to the db
        /// </summary>
        /// <param name="useraccount">useraccount class object from the textboxes</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(UserAccount useraccount)
        {
            bool uniqueViolation = db.UserAccounts.Any(x => (x.email == useraccount.email || x.userName == useraccount.userName) 
                                                && x.userId != useraccount.userId);

            if (ModelState.IsValid && !uniqueViolation)
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
            ViewBag.Warning = "Email And User Name must be unique";
            return View(useraccount);
        }

        /// <summary>
        /// This is the method to go the delete view, shows the details of the user he wants to delete
        /// </summary>
        /// <param name="useraccount"></param>
        public ActionResult Delete(int id = 0)
        {
            UserAccount useraccount = db.UserAccounts.Find(id);
            if (useraccount == null)
            {
                return HttpNotFound();
            }
            return View(useraccount);
        }

        /// <summary>
        /// This method deletes the user. Checks if the user has childFK's in other tables. If it has it cannot be deleted.
        /// </summary>
        /// <param name="useraccount">Primary id of useraccount</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserAccount useraccount = db.UserAccounts.Find(id);
            bool childCustomerSurveyExists = false;
            bool childGenericSurveyExists = false;

            if (useraccount.isTeacher)
            {
                childGenericSurveyExists = db.GenericSurveys.Any(z => z.supervisorUserId == useraccount.userId);
                childCustomerSurveyExists = db.CustomerSurveys.Any(x => x.supervisorUserId == useraccount.userId);
            }
            else 
            {
                childCustomerSurveyExists = db.CustomerSurveys.Any(y => y.observerUserId == useraccount.userId);
            }

            if (!childCustomerSurveyExists && !childGenericSurveyExists)
            {
                db.UserAccounts.Remove(useraccount);
                db.SaveChanges();
            }
            else 
            {
                TempData["Message"] = String.Format("Cannot delete '{0}' because the user is participating in a survey.",
                                                        useraccount.userName);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Releases all the used memory.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}