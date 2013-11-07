﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UltimateSecuritySurvey.Models;
namespace UltimateSecuritySurvey.Controllers
{
    public class HomeController : Controller
    {
   

        SecuritySurveyEntities entities = new SecuritySurveyEntities();



        //
        // GET: /Home/

        public ActionResult Index()
        {
            //Get five last questions in a list
            var DescendingQuestions = entities.Questions.OrderByDescending(x => x.questionId).ToList();
            int questionCount = (DescendingQuestions.Count >= 5) ? 5 : DescendingQuestions.Count;
            var lastFiveQuestions = DescendingQuestions.GetRange(0, questionCount);

            //Get five last categories in a list
            var DescendingCategories = entities.QuestionCategories.OrderByDescending(y => y.categoryId).ToList();
            int categoryCount = (DescendingCategories.Count >= 5) ? 5 : DescendingCategories.Count;
            var lastFiveCategories = DescendingCategories.GetRange(0, categoryCount);


            FrontPageModel frontPageView = new FrontPageModel(lastFiveQuestions, lastFiveCategories);
            return View(frontPageView);
        }

    }
}
