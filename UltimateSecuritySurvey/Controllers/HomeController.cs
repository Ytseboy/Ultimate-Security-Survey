using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UltimateSecuritySurvey.Models;

namespace UltimateSecuritySurvey.Controllers
{
    /// <summary>
    /// This controller is for the front page
    /// </summary>
    public class HomeController : Controller
    {
        SecuritySurveyEntities entities = new SecuritySurveyEntities();

        /// <summary>
        /// This is our Index method
        /// </summary>
        /// <returns>It returns five questions and five categories, both in descending order</returns>
        //
        // GET: /Home/
        public ActionResult Index()
        {
            //Get five last questions in a list using linq
            List<Question> lastFiveQuestions = entities.Questions.OrderByDescending(x => x.questionId).Take(5).ToList();
            
            //OLD VERSION
            //int questionCount = (DescendingQuestions.Count >= 5) ? 5 : DescendingQuestions.Count;
            //var lastFiveQuestions = DescendingQuestions.GetRange(0, questionCount);

            //Get five last categories in a list
            List<QuestionCategory> lastFiveCategories = entities.QuestionCategories.OrderByDescending(y => y.categoryId).Take(5).ToList();
            
            //OLD VERSION
            //int categoryCount = (DescendingCategories.Count >= 5) ? 5 : DescendingCategories.Count;
            //var lastFiveCategories = DescendingCategories.GetRange(0, categoryCount);

            FrontPageModel frontPageView = new FrontPageModel(lastFiveQuestions, lastFiveCategories);
            return View(frontPageView);
        }
    }
}
