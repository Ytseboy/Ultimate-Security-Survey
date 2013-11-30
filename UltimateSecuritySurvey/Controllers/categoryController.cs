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
    /// This controller to Display Categories and to manage them
    /// </summary>
    public class CategoryController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        /// <summary>
        /// This method to list the existing categories.
        /// Here is also a view bag to display delete errors
        /// </summary>
        //
        // GET: /Category/
        public ActionResult Index()
        {
            ViewBag.Error = TempData["Message"];
            return View(db.QuestionCategories.ToList());
        }

        /// <summary>
        /// This method finds one category and shows it individually with details.
        /// </summary>
        /// <param name="id">Primary of Question Category</param>
        //
        // GET: /category/Details/5
        public ActionResult Details(int id = 0)
        {
            QuestionCategory questioncategory = db.QuestionCategories.Find(id);
            if (questioncategory == null)
            {
                return HttpNotFound();
            }
            return View(questioncategory);
        }

        /// <summary>
        /// This method to go to the create view(note: We use same view for the create and edit.)
        /// </summary>
        //
        // GET: /category/Create
        public ActionResult Create()
        {
            return View("CreateEdit", new QuestionCategory());
        }

        /// <summary>
        /// This method to edit view, finds the category details
        /// </summary>
        //
        // GET: /category/Edit/5
        public ActionResult Edit(int id = 0)
        {
            QuestionCategory questioncategory = db.QuestionCategories.Find(id);
            if (questioncategory == null)
            {
                return HttpNotFound();
            }
            return View("CreateEdit",questioncategory);
        }

        /// <summary>
        /// This method to create/edit the category details
        /// </summary>
        /// <param name="questioncategory">category object</param>
        //
        // POST: /category/Edit/5
        [HttpPost]
        public ActionResult CreateEdit(QuestionCategory questioncategory)
        {
            bool uniqueViolation = db.QuestionCategories.Any(x => x.categoryName == questioncategory.categoryName
                                    && x.categoryId != questioncategory.categoryId);

            if (ModelState.IsValid && !uniqueViolation)
            {
                //No Id => Add
                if (questioncategory.categoryId <= 0)
                {
                    db.QuestionCategories.Add(questioncategory);
                }
                //Is Id => Update
                else
                {
                    db.Entry(questioncategory).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Warning = "Name must be unique!";
            return View(questioncategory);
        }

        /// <summary>
        /// This method to show chosen category details and confirm delete
        /// </summary>
        //
        // GET: /category/Delete/5
        public ActionResult Delete(int id = 0)
        {
            QuestionCategory questioncategory = db.QuestionCategories.Find(id);
            if (questioncategory == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest())           
                return PartialView("DeletePartial", questioncategory);

            return View(questioncategory);
        }

        /// <summary>
        /// This method to delete the existing category
        /// </summary>
        /// <param name="id">The primary key of the category</param>
        //
        // POST: /category/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            QuestionCategory questioncategory = db.QuestionCategories.Find(id);
            bool childExist = db.Questions.Any(x => x.categoryId == questioncategory.categoryId);

            if (!childExist)
            {
                db.QuestionCategories.Remove(questioncategory);
                db.SaveChanges();
            }
            else
            {
                TempData["Message"] = String.Format("Cannot delete '{0}' category because related questions exist!!",
                                                        questioncategory.categoryName);
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