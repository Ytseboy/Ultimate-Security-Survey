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
    /// Class to Create/Edit/Delete Answer options for questions
    /// </summary>
    public class AnswerOptionController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        //
        // GET: /AnswerOption/Create
        /// <summary>
        /// returns create view
        /// </summary>
        /// <param name="id">question id</param>
        /// <returns>CreateEdit</returns>
        public ActionResult Create(int id = 0)
        {
            //ensures that new answerOption is related to a question
            bool relatedQuestionExist = db.Questions.Any( x => x.questionId == id);
            
            if (!relatedQuestionExist)
            {
                return HttpNotFound();
            }

            AnswerOption answeroption = new AnswerOption { questionId = id };
            return View("CreateEdit", answeroption);
        }

        //
        // GET: /AnswerOption/Edit/5
        /// <summary>
        /// Edit view
        /// </summary>
        /// <param name="id">question id</param>
        /// <param name="number">answer option number</param>
        /// <returns>CreateEdit</returns>
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
        /// <summary>
        /// To Add or Update Answer Options
        /// </summary>
        /// <param name="answeroption">new or modified object</param>
        /// <returns>Question Details</returns>
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
                return RedirectToAction("Details", "Question", new { id = answeroption.questionId });
            }
            return View(answeroption);
        }

        //
        // GET: /AnswerOption/Delete/5
        /// <summary>
        /// Goes to Delete view
        /// </summary>
        /// <param name="id">question id</param>
        /// <param name="number">answer number</param>
        /// <returns>Delete view</returns>
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
        /// <summary>
        /// Deletes an item from db
        /// </summary>
        /// <param name="id">question id</param>
        /// <param name="number">answer number</param>
        /// <returns>Question Details</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id = 0, int number = 0)
        {
            //TODO: what if answer option was user in Customer Answer??

            AnswerOption answeroption = db.AnswerOptions.Find(id, number);
            db.AnswerOptions.Remove(answeroption);
            db.SaveChanges();

            return RedirectToAction("Details", "Question", new { id = id});
        }

        /// <summary>
        /// Disposes the entities
        /// </summary>
        /// <param name="disposing">bool</param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}