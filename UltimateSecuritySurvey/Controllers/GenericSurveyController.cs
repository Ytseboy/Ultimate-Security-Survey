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
    /// Controller handles Generic surveys and include/exclude Questions
    /// </summary>
    public class GenericSurveyController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        /// <summary>
        /// Returns all generic surveys
        /// </summary>
        /// <returns>Index view</returns>
        //
        // GET: /GenericSurvey/
        public ActionResult Index()
        {
            return View(db.GenericSurveys.ToList());
        }
     
        /// <summary>
        /// Shows deteils and related questions
        /// </summary>
        /// <param name="id">generic survey id</param>
        /// <returns>details view</returns>
        //
        // GET: /GenericSurvey/Details/5
        public ActionResult Details(string id = null)
        {
            GenericSurvey genericsurvey = db.GenericSurveys.Find(id);
            if (genericsurvey == null)
            {
                return HttpNotFound();
            }
            return View(genericsurvey);
        }
        
        /// <summary>
        /// To create new Generic survey and attach questions to it
        /// </summary>
        /// <returns>Create view</returns>
        //
        // GET: /GenericSurvey/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Adds new generic survey entry into database
        /// </summary>
        /// <param name="genericsurvey">generic survey object</param>
        /// <returns>Index redirect to action</returns>
        //
        // POST: /GenericSurvey/Create
        [HttpPost]
        public ActionResult Create(GenericSurvey genericsurvey)
        {
            if (ModelState.IsValid)
            {
                db.GenericSurveys.Add(genericsurvey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(genericsurvey);
        }

        /// <summary>
        /// Form to edit generic surveys
        /// </summary>
        /// <param name="id">generic survey Id</param>
        /// <returns>edit view</returns>
        //
        // GET: /GenericSurvey/Edit/5
        public ActionResult Edit(string id = null)
        {
            GenericSurvey genericsurvey = db.GenericSurveys.Find(id);
            if (genericsurvey == null)
            {
                return HttpNotFound();
            }
            return View(genericsurvey);
        }

        /// <summary>
        /// Submit changes and Update database
        /// </summary>
        /// <param name="genericsurvey">generic survey object</param>
        /// <returns>redirect to Index method</returns>
        //
        // POST: /GenericSurvey/Edit/5
        [HttpPost]
        public ActionResult Edit(GenericSurvey genericsurvey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genericsurvey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genericsurvey);
        }
    
        /// <summary>
        /// Delete confirm form
        /// </summary>
        /// <param name="id">generic survey id</param>
        /// <returns>delete view</returns>
        //
        // GET: /GenericSurvey/Delete/5
        public ActionResult Delete(string id = null)
        {
            GenericSurvey genericsurvey = db.GenericSurveys.Find(id);
            if (genericsurvey == null)
            {
                return HttpNotFound();
            }
            return View(genericsurvey);
        }

        /// <summary>
        /// Submit changes, deletes entry from DB
        /// </summary>
        /// <param name="id">generic survey Id</param>
        /// <returns>redirect to Index action</returns>
        //
        // POST: /GenericSurvey/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            GenericSurvey genericsurvey = db.GenericSurveys.Find(id);
            db.GenericSurveys.Remove(genericsurvey);
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