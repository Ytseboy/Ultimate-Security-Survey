using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UltimateSecuritySurvey.Models;

namespace UltimateSecuritySurvey.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly SecuritySurveyEntities db = new SecuritySurveyEntities();
        public ActionResult Index(string searchString)
        {
            var questions = from question in db.Questions
                            select question;

            if (!String.IsNullOrEmpty(searchString) && searchString.Length > 1)
            {
                questions = questions.Where(s => s.questionTextMain.Contains(searchString));
                ViewBag.query = searchString;
            }
            
            return View(questions);
        }

    }
}
