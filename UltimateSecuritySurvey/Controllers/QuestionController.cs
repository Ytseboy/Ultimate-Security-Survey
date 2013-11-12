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
    /// 
    /// </summary>
    public class QuestionController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //
        // GET: /Question/
        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.QuestionCategory).Include(q => q.QuestionType);
            return View(questions.ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //
        // GET: /Question/Details/5
        public ActionResult Details(int id = 0)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //
        // GET: /Question/Create
        public ActionResult Create()
        {
            ViewBag.categoryId = new SelectList(db.QuestionCategories, "categoryId", "categoryName");
            ViewBag.questionTypeId = new SelectList(db.QuestionTypes, "questionTypeId", "questionTypeName");
            return View("CreateEdit", new Question());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //
        // POST: /Question/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //TODO: Add logic concerning the  Generic Surveys

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