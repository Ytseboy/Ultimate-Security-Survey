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
        private enum AnswerStatus : int {NotSet, Reported, Commented, Validated};

        public ActionResult Index(int id = 0)
        {
            CustomerSurvey customerSurvey = db.CustomerSurveys.Find(id);
            if (customerSurvey == null)
                return HttpNotFound();

            //Only participating users can see/modify Report
            string currentUser = User.Identity.Name;
            if (currentUser != customerSurvey.UserAccount.userName && currentUser != customerSurvey.UserAccount1.userName)
            {
                TempData["Message"] = "Oops! Only survey observer and supervisor can access the survey report";
                return RedirectToAction("Index", "CustomerSurvey");
            }

            List<CustomerAnswer> answerList;
            List<Question> questionList;

            //Teacher see only created by student answers
            answerList = customerSurvey.CustomerAnswers.ToList();
            questionList = customerSurvey.GenericSurvey.Questions.ToList();

            //Additional Info to Display
            #region SidebarInfo
            ViewBag.SurveyTitle = customerSurvey.customerSurveyTitle;

            int questionsAmount = questionList.Count;
            int questionsAnswered = answerList.Count;
            ViewBag.AnsweredQuestions = String.Format("{0} / {1}", questionsAnswered, questionsAmount);

            double avgObserverStatus = (answerList.Average(x => x.observerStatusValue)) ?? 0;
            ViewBag.AverageObserverStatus = Math.Round(avgObserverStatus, 2);
            
            /* Factor here derrives from AnswerStatusValue Range. 
             * Minima = 0, Maxima = 4 --> Four answers with value 4 should give 100 %
             * --> 16/4 * factor = 100 % --> factor = 25 */
            int factor = 25;
            double averageAnswerValue = (answerList.Average(z => z.answerStatusValue)) ?? 0;
            int surveyProgress = (int)Math.Round(averageAnswerValue * factor);
            ViewBag.SurveyProgress = String.Format("{0}%", surveyProgress);
            #endregion

            if (User.IsInRole("Teacher"))
            {
                return View("IndexTeacher", answerList);
            }
            else
            {
                ViewBag.surveyId = customerSurvey.surveyId;
                return View("IndexStudent", questionList);
            }           
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(CustomerAnswer customeranswer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CustomerAnswers.Add(customeranswer);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(customeranswer);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(CustomerAnswer customeranswer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(customeranswer).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(customeranswer);
        //}

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
        [ValidateAntiForgeryToken]
        public ActionResult Validate(CustomerAnswer answer)
        {
            if (ModelState.IsValid)
            {
                answer.supervisorCommentDateAndTime = DateTime.Now;
                answer.answerStatusValue = (int)AnswerStatus.Validated;

                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();                
            }
            return RedirectToAction("Index", new { id = answer.surveyId, number = answer.questionId });
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}