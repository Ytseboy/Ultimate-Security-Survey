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
    /// Controller to handle customers surveys
    /// </summary>
    [Authorize]
    public class CustomerSurveyController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        //
        // GET: /CustomerSurvey/
        [Authorize(Roles = "Teacher, Student")]
        public ActionResult Index()
        {
            ViewBag.Error = TempData["Message"];

            var customersurveys = db.CustomerSurveys.Include(c => c.Customer).Include(c => c.GenericSurvey)
                        .Include(c => c.UserAccount).Include(c => c.UserAccount1).ToList();

            return View(customersurveys);
        }

        //
        // GET: /CustomerSurvey/Details/5
        [Authorize(Roles = "Teacher, Student")]
        public ActionResult Details(int id = 0)
        {
            CustomerSurvey customersurvey = db.CustomerSurveys.Find(id);
            if (customersurvey == null)
            {
                return HttpNotFound();
            }

            //Additional Info For Sidebar
            var surveyReport = customersurvey.CustomerAnswers;

            int questionsAmount = customersurvey.GenericSurvey.Questions.Count;
            int questionsAnswered = surveyReport.Count;
            ViewBag.AnsweredQuestions = String.Format("{0} / {1}", questionsAnswered, questionsAmount);
            
            double avgObserverStatus = (surveyReport.Average(x => x.observerStatusValue)) ?? 0;
            ViewBag.AverageObserverStatus = Math.Round(avgObserverStatus, 2);

            /* Factor here derrives from AnswerStatusValue Range. 
             * Minima = 0, Maxima = 4 --> Four answers with value 4 should give 100 %
             * --> 16/4 * factor = 100 % --> factor = 25 */
            int factor = 25;
            double averageAnswerValue = (surveyReport.Average(z => z.answerStatusValue)) ?? 0;
            int surveyProgress = (int)Math.Round(averageAnswerValue * factor);
            ViewBag.SurveyProgress = String.Format("{0}%", surveyProgress);

            return View(customersurvey);
        }

        //
        // GET: /CustomerSurvey/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            var students = db.UserAccounts.Where(x => !x.isTeacher).ToList();
            var teachers = db.UserAccounts.Where(x => x.isTeacher).ToList();

            ViewBag.customerId = new SelectList(db.Customers, "customerId", "companyName");
            ViewBag.baseSurveyId = new SelectList(db.GenericSurveys, "surveyId", "title");
            ViewBag.observerUserId = new SelectList(students, "userId", "userName");
            ViewBag.supervisorUserId = new SelectList(teachers, "userId", "userName", User.Identity.Name);
            return View("CreateEdit", new CustomerSurvey());
        }

        //
        // GET: /CustomerSurvey/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int id = 0)
        {
            CustomerSurvey customersurvey = db.CustomerSurveys.Find(id);
            if (customersurvey == null)
            {
                return HttpNotFound();
            }
            var students = db.UserAccounts.Where(x => !x.isTeacher).ToList();

            ViewBag.customerId = new SelectList(db.Customers, "customerId", "companyName", customersurvey.customerId);
            ViewBag.baseSurveyId = new SelectList(db.GenericSurveys, "surveyId", "title", customersurvey.baseSurveyId);
            ViewBag.observerUserId = new SelectList(students, "userId", "userName", customersurvey.observerUserId);
            return View("CreateEdit", customersurvey);
        }

        //
        // POST: /CustomerSurvey/Edit/5
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult CreateEdit(CustomerSurvey customersurvey)
        {
            if (ModelState.IsValid)
            {
                //If No ID => ADD
                if (customersurvey.surveyId <= 0)
                {
                    db.CustomerSurveys.Add(customersurvey);
                }
                //IF there is ID => Update
                else
                {
                    db.Entry(customersurvey).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var students = db.UserAccounts.Where(x => !x.isTeacher).ToList();
            var teachers = db.UserAccounts.Where(x => x.isTeacher).ToList();

            ViewBag.customerId = new SelectList(db.Customers, "customerId", "companyName", customersurvey.customerId);
            ViewBag.baseSurveyId = new SelectList(db.GenericSurveys, "surveyId", "title", customersurvey.baseSurveyId);
            ViewBag.observerUserId = new SelectList(students, "userId", "userName", customersurvey.observerUserId);
            ViewBag.supervisorUserId = new SelectList(teachers, "userId", "userName", customersurvey.supervisorUserId);
            return View(customersurvey);
        }

        //
        // GET: /CustomerSurvey/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int id = 0)
        {
            CustomerSurvey customersurvey = db.CustomerSurveys.Find(id);
            if (customersurvey == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return PartialView(customersurvey);

            return View(customersurvey);
        }

        //
        // POST: /CustomerSurvey/Delete/5
        [Authorize(Roles = "Teacher")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerSurvey customersurvey = db.CustomerSurveys.Find(id);
            bool credentials = customersurvey.UserAccount1.userName == User.Identity.Name;

            if (credentials)
            {
                db.CustomerSurveys.Remove(customersurvey);
                db.SaveChanges();
            }
            else 
            {
                TempData["Message"] = "Only the supervising teacher may delete this survey.";
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