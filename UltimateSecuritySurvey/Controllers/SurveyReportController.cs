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

            //Teacher see only created by student answers
            List<CustomerAnswer> answerList = customerSurvey.CustomerAnswers.ToList();
            List<Question> questionList = customerSurvey.GenericSurvey.Questions.ToList();
            ViewBag.SurveyTitle = customerSurvey.customerSurveyTitle;

            //Additional Info to Display
            #region SidebarInfo

            int questionsAmount = questionList.Count;
            int questionsAnswered = answerList.Where(a => a.answerStatusValue > (int)AnswerStatus.NotSet).Count();
            int questionsValidated = answerList.Where(a => a.answerStatusValue == (int)AnswerStatus.Validated).Count();
            ViewBag.AnsweredQuestions = String.Format("{0} / {1}", questionsAnswered, questionsAmount);
            ViewBag.ValidatedQuestions = String.Format("{0} / {1}", questionsValidated, questionsAmount);

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

        [Authorize(Roles = "Student")]
        public ActionResult Submit(int id = 0, int number = 0)
        {
            //That ensures that parameters passed are correct
            CustomerSurvey survey = db.CustomerSurveys.Find(id);
            if (survey == null)
                return HttpNotFound();

            CustomerAnswer answer = survey.CustomerAnswers.SingleOrDefault(a => a.questionId == number);
            if (answer == null)
            {
                /*if answer does not exist --> check if question is part of base survey 
                 * (ensures that parameters passed are correct)*/
                bool includedInBaseSurvey = survey.GenericSurvey.Questions.Any(q => q.questionId == number);
                if (!includedInBaseSurvey)
                {
                    return HttpNotFound();
                }
                //If question is included --> create new Dummy customer Answer
                else
                {
                    answer = new CustomerAnswer() { surveyId = id, questionId = number, answerText = "-", observerStatusValue = 0 };
                    db.CustomerAnswers.Add(answer);
                    db.SaveChanges();
                }
            }

            #region Additional info
            //Question type into account (1: long, 2: short;)
            ViewBag.QuestionType = answer.Question.questionTypeId;
            //Drop downs
            ViewBag.answerOptionNumber = 
                new SelectList(answer.Question.AnswerOptions, "answerNumber", "answerText", answer.answerOptionNumber);
            ViewBag.observerStatusValue = 
                new SelectList(db.ObserverStatus, "statusValue", "description", answer.observerStatusValue);

            int measuresAmount = answer.Question.GenericCountermeasures.Count;
            ViewBag.MeasuresAmount = measuresAmount > 3 ? 3 : measuresAmount;
                
            ViewBag.countermeasureId1 = 
                new SelectList(answer.Question.GenericCountermeasures, "countermeasureId", "title", answer.countermeasureId1);
            ViewBag.countermeasureId2 =
                new SelectList(answer.Question.GenericCountermeasures, "countermeasureId", "title", answer.countermeasureId2);
            ViewBag.countermeasureId3 =
                new SelectList(answer.Question.GenericCountermeasures, "countermeasureId", "title", answer.countermeasureId3);

            #endregion

            if (Request.IsAjaxRequest())
                return PartialView(answer);

            return View(answer);
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(CustomerAnswer answer)
        {

            //    if (ModelState.IsValid)
            //    {
            //        db.Entry(customeranswer).State = EntityState.Modified;
            //        db.SaveChanges();
            //        return RedirectToAction("Index");
            //    }
            //    return View(customeranswer);

            //Automatique Date And answer Status processing

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}