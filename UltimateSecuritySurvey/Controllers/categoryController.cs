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
    public class categoryController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        //
        // GET: /category/

        public ActionResult Index()
        {
            return View(db.QuestionCategories.ToList());
        }

        //
        // GET: /category/Details/5

        public ActionResult Details(string id = null)
        {
            QuestionCategory questioncategory = db.QuestionCategories.Find(id);
            if (questioncategory == null)
            {
                return HttpNotFound();
            }
            return View(questioncategory);
        }

        //
        // GET: /category/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /category/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionCategory questioncategory)
        {
            if (ModelState.IsValid)
            {
                db.QuestionCategories.Add(questioncategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(questioncategory);
        }

        //
        // GET: /category/Edit/5

        public ActionResult Edit(string id = null)
        {
            QuestionCategory questioncategory = db.QuestionCategories.Find(id);
            if (questioncategory == null)
            {
                return HttpNotFound();
            }
            return View(questioncategory);
        }

        //
        // POST: /category/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionCategory questioncategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questioncategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(questioncategory);
        }

        //
        // GET: /category/Delete/5

        public ActionResult Delete(string id = null)
        {
            QuestionCategory questioncategory = db.QuestionCategories.Find(id);
            if (questioncategory == null)
            {
                return HttpNotFound();
            }
            return View(questioncategory);
        }

        //
        // POST: /category/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            QuestionCategory questioncategory = db.QuestionCategories.Find(id);
            db.QuestionCategories.Remove(questioncategory);
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