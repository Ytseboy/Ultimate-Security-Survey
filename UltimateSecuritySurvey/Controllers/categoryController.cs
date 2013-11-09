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
    public class CategoryController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        /// <summary>
        /// This method to list the existing categories.
        /// Here is also a view bag to display delete errors
        /// </summary>
        //
        // GET: /category/
        public ActionResult Index()
        {
            ViewBag.DeleteError = (TempData["Message"]) ?? string.Empty;
            return View(db.QuestionCategories.ToList());
        }

        /// <summary>
        /// This method finds one category and shows it individually with details.
        /// </summary>
        /// <param name="id">Primary of Question Category</param>
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

        /// <summary>
        /// This method to go to the create view
        /// </summary>
        //
        // GET: /category/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// This method to add new categories
        /// </summary>
        /// <param name="questioncategory">Question category object from textbox</param>
        //
        // POST: /category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionCategory questioncategory)
        {
            bool uniqueViolation = db.QuestionCategories.Any(x => x.categoryId == questioncategory.categoryId)
                                || db.QuestionCategories.Any(x => x.categoryName == questioncategory.categoryName);

            if (ModelState.IsValid && !uniqueViolation)
            {
                db.QuestionCategories.Add(questioncategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Id and Name must be unique!";
            return View(questioncategory);
        }

        /// <summary>
        /// This method to edit view, finds the category details
        /// </summary>
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

        /// <summary>
        /// This method to edit the category details, cant change the id
        /// </summary>
        /// <param name="questioncategory">Existing category object</param>
        //
        // POST: /category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionCategory questioncategory)
        {
            bool uniqueViolation = db.QuestionCategories
                .Any(x => x.categoryName == questioncategory.categoryName 
                    && x.categoryId != questioncategory.categoryId);

            if (ModelState.IsValid && !uniqueViolation)
            {
                db.Entry(questioncategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Name must be unique!";
            return View(questioncategory);
        }

        /// <summary>
        /// This method to show chosen category details and confirm delete
        /// </summary>
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

        /// <summary>
        /// This method to delete the existing category
        /// </summary>
        /// <param name="id">The primary key of the category</param>
        //
        // POST: /category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            QuestionCategory questionCategory = db.QuestionCategories.Find(id);
            bool childExist = db.Questions.Any(x => x.categoryId == questionCategory.categoryId);

            if (!childExist)
            {
                db.QuestionCategories.Remove(questionCategory);
                db.SaveChanges();
            }
            else
            {
                TempData["Message"] = "Cannot delete this Category because related questions exist!!";
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