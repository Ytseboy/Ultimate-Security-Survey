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
    public class GenericSurveyController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        //
        // GET: /GenericSurvey/

        public ActionResult Index()
        {
            return View(db.GenericSurveys.ToList());
        }

        //
        // GET: /GenericSurvey/Details/5

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

        public ActionResult Create()
        {
            return View("CreateEdit", new GenericSurvey());
        }

        //
        // GET: /GenericSurvey/Edit/5

        public ActionResult Edit(int id = 0)
        {
            GenericSurvey genericsurvey = db.GenericSurveys.Find(id);
            if (genericsurvey == null)
            {
                return HttpNotFound();
            }
            return View("CreateEdit", genericsurvey);
        }

        //
        // POST: /GenericSurvey/Edit/5

        [HttpPost]
        public ActionResult CreateEdit(GenericSurvey genericsurvey)
        {
            if (ModelState.IsValid)
            {
                //No Id => Add
                if (genericsurvey.surveyId <= 0)
                {
                    db.GenericSurveys.Add(genericsurvey);
                }
                //Is Id => Update
                else
                {
                    db.Entry(genericsurvey).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genericsurvey);
        }

        //
        // GET: /GenericSurvey/Delete/5

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

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
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