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
    [Authorize]
    public class HomeController : Controller
    {
        SecuritySurveyEntities db = new SecuritySurveyEntities();

        /// <summary>
        /// This is our Index method
        /// </summary>
        /// <returns>It returns five surveys, customers and users, all in descending order</returns>
        //
        // GET: /Home/
        public ActionResult Index()
        {
            //Get five last Customer surveys
            List<CustomerSurvey> lastFiveCustomerSurveys = db.CustomerSurveys.OrderByDescending(x => x.surveyId).Take(5).ToList();
            //Get five last Customers
            List<Customer> lastFiveCustomers = db.Customers.OrderByDescending(x => x.customerId).Take(5).ToList();
            List<UserAccount> lastFiveUsers = db.UserAccounts.OrderByDescending(x => x.userId).Take(5).ToList();

            FrontPageModel frontPageView = new FrontPageModel(lastFiveCustomerSurveys, lastFiveCustomers, lastFiveUsers);

            //Stats
            Dictionary<string, int> stats = new Dictionary<string, int>();
            stats.Add("Users", db.UserAccounts.Count());
            stats.Add("Customers", db.Customers.Count());
            stats.Add("Surveys", db.CustomerSurveys.Count());
            stats.Add("Base Surveys", db.GenericSurveys.Count());
            stats.Add("Questions", db.Questions.Count());

            ViewBag.Stats = stats;

            return View(frontPageView);
        }
    }
}
