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
    public class QuestionController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        //
        // GET: /Question/

        public ActionResult Index()
        {
            return View(db.Questions.ToList());
        }

        //
        // GET: /Question/Details/5

        public ActionResult Details(string id = null)
        {
            Question question = db.Questions.Find(id);

            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // GET: /Question/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Question/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question)
        {
            bool uniqueViolation = db.Questions.Any(x => x.questionId == question.questionId);

            if (ModelState.IsValid && !uniqueViolation)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.message = "Id must be unique!";
            return View(question);
        }

        //
        // GET: /Question/Edit/5

        public ActionResult Edit(string id = null)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // POST: /Question/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        //
        // GET: /Question/Delete/5

        public ActionResult Delete(string id = null)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // POST: /Question/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Question question = db.Questions.Find(id);

            db.Questions.Remove(question);
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