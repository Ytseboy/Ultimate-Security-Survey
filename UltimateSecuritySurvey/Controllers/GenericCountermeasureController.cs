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
    /// Class to manage counter measures
    /// </summary>
    public class GenericCountermeasureController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        //
        // GET: /GenericCountermeasure/Create
        /// <summary>
        /// Create view
        /// </summary>
        /// <returns>Create view</returns>
        public ActionResult Create(int id = 0)
        {
            Question parentQuestion = db.Questions.Find(id);

            if (parentQuestion == null)
            {
                return HttpNotFound();
            }

            ViewBag.motherCountermeasure = new SelectList(db.GenericCountermeasures, "countermeasureId", "title");
            GenericCountermeasure genericCountermeasure = 
                            new GenericCountermeasure{ questionId = id, Question = parentQuestion, dateAndTime = DateTime.Now};
            return View("CreateEdit", genericCountermeasure);
        }

        //
        // GET: /GenericCountermeasure/Edit/5
        /// <summary>
        /// Edit view
        /// </summary>
        /// <param name="id">countermeasure id</param>
        /// <returns>CreateEdit</returns>
        public ActionResult Edit(int id = 0)
        {
            GenericCountermeasure genericcountermeasure = db.GenericCountermeasures.Find(id);
            if (genericcountermeasure == null)
            {
                return HttpNotFound();
            }
            ViewBag.motherCountermeasure = new SelectList(db.GenericCountermeasures, "countermeasureId", "title", genericcountermeasure.motherCountermeasure);
            ViewBag.questionId = new SelectList(db.Questions, "questionId", "questionTextMain", genericcountermeasure.questionId);
            return View("CreateEdit", genericcountermeasure);
        }

        //
        // POST: /GenericCountermeasure/Edit/5
        /// <summary>
        /// Adds or updates entry in Generic countermeasure
        /// </summary>
        /// <param name="genericcountermeasure">object</param>
        /// <returns>Question details</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(GenericCountermeasure genericcountermeasure)
        {
            if (ModelState.IsValid)
            {
                //No id => Add
                if (genericcountermeasure.countermeasureId <= 0)
                {
                    db.GenericCountermeasures.Add(genericcountermeasure);
                }
                //Is Id => Update
                else
                {
                    db.Entry(genericcountermeasure).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                return RedirectToAction("Details", "Question", new { id = genericcountermeasure.questionId});
            }

            ViewBag.motherCountermeasure = new SelectList(db.GenericCountermeasures, "countermeasureId", "title", genericcountermeasure.motherCountermeasure);
            return View(genericcountermeasure);
        }

        //
        // GET: /GenericCountermeasure/Delete/5
        /// <summary>
        /// Delete view for genericcountermeasure
        /// </summary>
        /// <param name="id">countermeasure id</param>
        /// <returns>delete view</returns>
        public ActionResult Delete(int id = 0)
        {
            GenericCountermeasure genericcountermeasure = db.GenericCountermeasures.Find(id);
            if (genericcountermeasure == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return PartialView(genericcountermeasure);

            return View(genericcountermeasure);
        }

        //
        // POST: /GenericCountermeasure/Delete/5
        /// <summary>
        /// Deletes the entry from db
        /// </summary>
        /// <param name="id">counter measure id</param>
        /// <returns>Question details</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GenericCountermeasure genericcountermeasure = db.GenericCountermeasures.Find(id);
            bool includedInCustomerAnswer = db.CustomerAnswers.Any(x => x.countermeasureId1 == id
                                                            || x.countermeasureId2 == id
                                                            || x.countermeasureId3 == id);

            if (!includedInCustomerAnswer)
            {
                db.GenericCountermeasures.Remove(genericcountermeasure);
                db.SaveChanges();
            }
            else
            {
                TempData["Message"] = String.Format("Cannot delete counter measure '{0}' because it was used in Customer Answer",
                                                            genericcountermeasure.title);
            }

            return RedirectToAction("Details", "Question", new { id = genericcountermeasure.questionId });
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">bool</param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}