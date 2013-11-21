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
    /// Page to manage surveys
    /// </summary>
    public class GenericSurveyController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        //
        // GET: /GenericSurvey/
        /// <summary>
        /// List of surveys
        /// </summary>
        /// <returns>Index view</returns>
        public ActionResult Index()
        {
            var genericsurveys = db.GenericSurveys.Include(g => g.UserAccount);
            return View(genericsurveys.ToList());
        }

        //
        // GET: /GenericSurvey/Details/5
        /// <summary>
        /// Details view
        /// </summary>
        /// <param name="id">survey id</param>
        /// <returns>Details view</returns>
        public ActionResult Details(int id = 0)
        {
            GenericSurvey genericsurvey = db.GenericSurveys.Find(id);
            if (genericsurvey == null)
            {
                return HttpNotFound();
            }
            return View(genericsurvey);
        }

        //
        // GET: /GenericSurvey/Create
        /// <summary>
        /// Create view
        /// </summary>
        /// <returns>CreateEdit</returns>
        public ActionResult Create()
        {
            ViewBag.supervisorUserId = new SelectList(db.UserAccounts, "userId", "firstName");
            return View("CreateEdit", new GenericSurvey());
        }

        //
        // GET: /GenericSurvey/Edit/5
        /// <summary>
        /// Edit view
        /// </summary>
        /// <param name="id">generic survey id</param>
        /// <returns>CreateEdit</returns>
        public ActionResult Edit(int id = 0)
        {
            GenericSurvey genericsurvey = db.GenericSurveys.Find(id);
            if (genericsurvey == null)
            {
                return HttpNotFound();
            }
            ViewBag.supervisorUserId = new SelectList(db.UserAccounts, "userId", "firstName", genericsurvey.supervisorUserId);
            return View("CreateEdit", genericsurvey);
        }

        //
        // POST: /GenericSurvey/Edit/5
        /// <summary>
        /// To add or update entry in db
        /// </summary>
        /// <param name="genericsurvey">object</param>
        /// <returns>Index view</returns>
        [HttpPost]
        public ActionResult CreateEdit(GenericSurvey genericsurvey)
        {
            if (ModelState.IsValid)
            {
                //If No Id => Add
                if (genericsurvey.surveyId <= 0)
                {
                    db.GenericSurveys.Add(genericsurvey);
                }
                //If IS Id => Update
                else
                {
                    db.Entry(genericsurvey).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.supervisorUserId = new SelectList(db.UserAccounts, "userId", "firstName", genericsurvey.supervisorUserId);
            return View(genericsurvey);
        }

        //
        // GET: /GenericSurvey/Delete/5
        /// <summary>
        /// Delete view
        /// </summary>
        /// <param name="id">survey id</param>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            GenericSurvey genericsurvey = db.GenericSurveys.Find(id);
            if (genericsurvey == null)
            {
                return HttpNotFound();
            }
            return View(genericsurvey);
        }

        //
        // POST: /GenericSurvey/Delete/5
        /// <summary>
        /// Post delete to delete entry
        /// </summary>
        /// <param name="id">survey id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GenericSurvey genericsurvey = db.GenericSurveys.Find(id);
            bool childExist = db.CustomerSurveys.Any(x => x.surveyId == id);

            if (!childExist)
            {
                db.GenericSurveys.Remove(genericsurvey);
                db.SaveChanges();
            }
            else
            {
                TempData["Message"] = "Cannot delete, because this survey was used as a Customer survey";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method to reveal resources
        /// </summary>
        /// <param name="disposing">bool</param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}