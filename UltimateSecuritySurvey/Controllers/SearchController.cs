using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UltimateSecuritySurvey.Models;

namespace UltimateSecuritySurvey.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/
        private readonly SecuritySurveyEntities db = new SecuritySurveyEntities();
        public ActionResult Index(string searchString)
        {
            var questions = from question in db.Questions
                            select question;

            if (!String.IsNullOrEmpty(searchString))
            {
                questions = questions.Where(s => s.questionTextMain.Contains(searchString));
            }

            return View(questions);
        }

    }
}
