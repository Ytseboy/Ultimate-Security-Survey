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
    /// This is the controller for Question page and managing questions
    /// </summary>
    public class QuestionController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        /// <summary>
        /// This method Gets list of existing questions from the database with Category and Question Type
        /// </summary>
        //
        // GET: /Question/
        public ActionResult Index()
        {
            ViewBag.Error = TempData["Message"];
            var questions = db.Questions.Include(q => q.QuestionCategory).Include(q => q.QuestionType);
            return View(questions.ToList());
        }

        /// <summary>
        /// This method finds one question and shows it in more detailed view
        /// </summary>
        /// <param name="id">Primary of Question</param>
        /// <returns>View of single question</returns>
        //
        // GET: /Question/Details/5
        public ActionResult Details(int id = 0)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.Error = TempData["Message"];
            return View(question);
        }

        /// <summary>
        /// Method to create new questions view(note: We use the same view for create and edit)
        /// </summary>
        /// <returns>Returns empty object</returns>
        //
        // GET: /Question/Create
        public ActionResult Create()
        {
            ViewBag.categoryId = new SelectList(db.QuestionCategories, "categoryId", "categoryName");
            ViewBag.questionTypeId = new SelectList(db.QuestionTypes, "questionTypeId", "questionTypeName");
            return View("CreateEdit", new Question());
        }

        /// <summary>
        /// Method to edit existing questions, finds the question details
        /// </summary>
        //
        // GET: /Question/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }

            ViewBag.categoryId = new SelectList(db.QuestionCategories, "categoryId", "categoryName", question.categoryId);
            ViewBag.questionTypeId = new SelectList(db.QuestionTypes, "questionTypeId", "questionTypeName", question.questionTypeId);
            return View("CreateEdit", question);
        }

        /// <summary>
        /// Method to insert or update database for question
        /// </summary>
        /// <param name="question">question object</param>
        /// <returns>View</returns>
        //
        // POST: /Question/Edit/5
        [HttpPost]
        public ActionResult CreateEdit(Question question)
        {
            if (ModelState.IsValid)
            {
                //No Id => Add
                if (question.questionId <= 0)
                {
                    db.Questions.Add(question);
                }
                //Is Id => Update
                else
                {
                    db.Entry(question).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categoryId = new SelectList(db.QuestionCategories, "categoryId", "categoryName", question.categoryId);
            ViewBag.questionTypeId = new SelectList(db.QuestionTypes, "questionTypeId", "questionTypeName", question.questionTypeId);
            return View(question);
        }

        /// <summary>
        /// Method to delete a question
        /// </summary>
        /// <param name="id">question id</param>
        /// <returns>View</returns>
        //
        // GET: /Question/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        /// <summary>
        /// Delete the entry from database
        /// </summary>
        /// <param name="id">question id</param>
        /// <returns>Index View</returns>
        //
        // POST: /Question/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            bool includedInSurvey = question.GenericSurveys.Count > 0;

            if (!includedInSurvey)
            {
                //Delete related Countermeasure
                int countMeasures = question.GenericCountermeasures.Count;
                var measuresList = question.GenericCountermeasures.ToList();

                for(int i = 0; i < countMeasures; i++)
                    db.GenericCountermeasures.Remove(measuresList[i]);

                //Delete actual question
                db.Questions.Remove(question);
                db.SaveChanges();
            }
            else
            {
                TempData["Message"] = String.Format("Cannot delete question '{0}' because it is included in survey!!", 
                                                    question.questionTextMain);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}