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
    /// This controller to Displays Categories and to manage them
    /// </summary>
    public class categoryController : Controller
    {

       
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        //
        // GET: /category/

        /// <summary>
        /// This method to list the existing categories.
        /// Here is also a view bag to display delete errors
        /// </summary>

        public ActionResult Index()
        {
            ViewBag.DeleteError = (TempData["DeleteError"]) ?? string.Empty;
            return View(db.QuestionCategories.ToList());
        }

        //
        // GET: /category/Details/5

        /// <summary>
        /// This method finds one category and shows it individually with details.
        /// </summary>
        /// <param name="id">Primary of Question Category</param>

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

        /// <summary>
        /// This method to go to the create view
        /// </summary>

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /category/Create

        /// <summary>
        /// This method to add new categories
        /// </summary>
        /// <param name="questioncategory">Question category object from textbox</param>
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

        /// <summary>
        /// This method to edit view, finds the category details
        /// </summary>

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
        /// <summary>
        /// This method to edit the category details, cant change the id
        /// </summary>
        /// <param name="questioncategory">Existing category object</param>

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
        /// <summary>
        /// This method to show chosen category details and confirm delete
        /// </summary>

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

        /// <summary>
        /// This method to delete the existing category
        /// </summary>
        /// <param name="id">The primary key of the category</param>

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            QuestionCategory questioncategory = db.QuestionCategories.Find(id);
            if (CheckIfQuestionsExistForCategory(questioncategory.categoryId))
            {
                db.QuestionCategories.Remove(questioncategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["DeleteError"] = "Cannot delete this Category because related questions exist!!";
                return RedirectToAction("Index");
            }
         }
        /// <summary>
        /// This method to Check if this category has questions in question table, in a case of delete violation
        /// </summary>
        /// <param name="id">The chosen category id</param>

        private bool CheckIfQuestionsExistForCategory(string id)
        {
            Question question = db.Questions.Where(x => x.categoryId == id).First();
            if (question == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}