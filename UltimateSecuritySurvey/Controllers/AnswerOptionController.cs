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
    public class AnswerOptionController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        

        //
        // GET: /AnswerOption/Create


        public ActionResult Create(int id = 0)
        {
            //To DO: Check that question with the id exists

            AnswerOption answeroption = new AnswerOption { questionId = id };
            return View("CreateEdit", answeroption);
        }

        
        //
        // GET: /AnswerOption/Edit/5

        public ActionResult Edit(int id = 0, int number = 0)
        {
            AnswerOption answeroption = db.AnswerOptions.Find(id, number);
            if (answeroption == null)
            {
                return HttpNotFound();
            }
            return View("CreateEdit", answeroption);
        }

        //
        // POST: /AnswerOption/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(AnswerOption answeroption)
        {
            if (ModelState.IsValid)
            {
                //No Id => Add
                if (answeroption.answerNumber <= 0)
                {
                    db.AnswerOptions.Add(answeroption);
                }
                //Is Id => Update
                else
                {
                    db.Entry(answeroption).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(answeroption);
        }

        //
        // GET: /AnswerOption/Delete/5

        public ActionResult Delete(int id = 0, int number = 0)
        {
            AnswerOption answeroption = db.AnswerOptions.Find(id, number);
            if (answeroption == null)
            {
                return HttpNotFound();
            }
            return View(answeroption);
        }

        //
        // POST: /AnswerOption/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AnswerOption answeroption = db.AnswerOptions.Find(id);
            db.AnswerOptions.Remove(answeroption);
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