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
    public class GenericCountermeasureController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        

        //
        // GET: /GenericCountermeasure/Create

        public ActionResult Create()
        {
            ViewBag.motherCountermeasure = new SelectList(db.GenericCountermeasures, "countermeasureId", "title");
            ViewBag.questionId = new SelectList(db.Questions, "questionId", "questionTextMain");
            return View();
        }

        //
        // POST: /GenericCountermeasure/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GenericCountermeasure genericcountermeasure)
        {
            if (ModelState.IsValid)
            {
                db.GenericCountermeasures.Add(genericcountermeasure);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.motherCountermeasure = new SelectList(db.GenericCountermeasures, "countermeasureId", "title", genericcountermeasure.motherCountermeasure);
            ViewBag.questionId = new SelectList(db.Questions, "questionId", "questionTextMain", genericcountermeasure.questionId);
            return View(genericcountermeasure);
        }

        //
        // GET: /GenericCountermeasure/Edit/5

        public ActionResult Edit(int id = 0)
        {
            GenericCountermeasure genericcountermeasure = db.GenericCountermeasures.Find(id);
            if (genericcountermeasure == null)
            {
                return HttpNotFound();
            }
            ViewBag.motherCountermeasure = new SelectList(db.GenericCountermeasures, "countermeasureId", "title", genericcountermeasure.motherCountermeasure);
            ViewBag.questionId = new SelectList(db.Questions, "questionId", "questionTextMain", genericcountermeasure.questionId);
            return View(genericcountermeasure);
        }

        //
        // POST: /GenericCountermeasure/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GenericCountermeasure genericcountermeasure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genericcountermeasure).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.motherCountermeasure = new SelectList(db.GenericCountermeasures, "countermeasureId", "title", genericcountermeasure.motherCountermeasure);
            ViewBag.questionId = new SelectList(db.Questions, "questionId", "questionTextMain", genericcountermeasure.questionId);
            return View(genericcountermeasure);
        }

        //
        // GET: /GenericCountermeasure/Delete/5

        public ActionResult Delete(int id = 0)
        {
            GenericCountermeasure genericcountermeasure = db.GenericCountermeasures.Find(id);
            if (genericcountermeasure == null)
            {
                return HttpNotFound();
            }
            return View(genericcountermeasure);
        }

        //
        // POST: /GenericCountermeasure/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GenericCountermeasure genericcountermeasure = db.GenericCountermeasures.Find(id);
            db.GenericCountermeasures.Remove(genericcountermeasure);
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