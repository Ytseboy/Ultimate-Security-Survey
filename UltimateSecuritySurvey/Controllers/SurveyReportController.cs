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
    [Authorize]
    public class SurveyReportController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        //
        // GET: /CustomerAnswer/

        public ActionResult Index(int id = 0)
        {
            CustomerSurvey customerSurvey = db.CustomerSurveys.Find(id);
            if (customerSurvey == null)
                return HttpNotFound();

            //Only participating users can see/modify Report
            if (User.Identity.Name != customerSurvey.UserAccount.userName && User.Identity.Name != customerSurvey.UserAccount1.userName)
            {
                TempData["Message"] = "Oops! Only survey observer and supervisor can access the survey report";
                return RedirectToAction("Index", "CustomerSurvey");
            }

            List<CustomerAnswer> answerList;

            //Teacher see only created by student answers
            answerList = customerSurvey.CustomerAnswers.ToList();

            //Additional Info to Display
            ViewBag.SurveyTitle = customerSurvey.customerSurveyTitle;
            ViewBag.QuestionsAmount = customerSurvey.GenericSurvey.Questions.Count;
            ViewBag.QuestionsAnswered = answerList.Count;

            double avgObserverStatus = (answerList.Average(x => x.observerStatusValue)) ?? 0;
            ViewBag.AverageObserverStatus = Math.Round(avgObserverStatus, 2);
            
            /* Factor here derrives from AnswerStatusValue Range. 
             * Minima = 0, Maxima = 4 --> Four answers with value 4 should give 100 %
             * --> 16/4 * factor = 100 % --> factor = 25 */
            int factor = 25;
            double averageAnswerValue = (answerList.Average(z => z.answerStatusValue)) ?? 0;
            int surveyProgress = (int)Math.Round(averageAnswerValue * factor);
            ViewBag.SurveyProgress = String.Format("{0}%", surveyProgress);

            return View("IndexTeacher", answerList);

            //LATER TO DO = STUDENT See all the questions included in generic survey
        }

        //
        // GET: /CustomerAnswer/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomerAnswer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerAnswer customeranswer)
        {
            if (ModelState.IsValid)
            {
                db.CustomerAnswers.Add(customeranswer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customeranswer);
        }

        //
        // GET: /CustomerAnswer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomerAnswer customeranswer = db.CustomerAnswers.Find(id);
            if (customeranswer == null)
            {
                return HttpNotFound();
            }
            return View(customeranswer);
        }

        //
        // POST: /CustomerAnswer/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerAnswer customeranswer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customeranswer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customeranswer);
        }

        //Teacher Validation GET
        [Authorize(Roles="Teacher")]
        public ActionResult Validate(int id = 0, int number = 0)
        {
            CustomerAnswer answer = db.CustomerAnswers.Find(id, number);
            if (answer == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return PartialView(answer);

            return View(answer);
        }

        //Teacher Validation POST
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult Validate(CustomerAnswer answer)
        {
            // TO DO later

            return View(answer);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}