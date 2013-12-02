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
        SecuritySurveyEntities entities = new SecuritySurveyEntities();

        /// <summary>
        /// This is our Index method
        /// </summary>
        /// <returns>It returns five surveys, customers and users, all in descending order</returns>
        //
        // GET: /Home/
        public ActionResult Index()
        {
            //Get five last Customer surveys
            List<CustomerSurvey> lastFiveCustomerSurveys = entities.CustomerSurveys.OrderByDescending(x => x.surveyId).Take(5).ToList();

            //Get five last Generic surveys
            List<GenericSurvey> lastFiveGenericSurveys = entities.GenericSurveys.OrderByDescending(x => x.surveyId).Take(5).ToList();

            //Get five last Customers
            List<Customer> lastFiveCustomers = entities.Customers.OrderByDescending(x => x.customerId).Take(5).ToList();

            List<UserAccount> lastFiveUsers = entities.UserAccounts.OrderByDescending(x => x.userId).Take(5).ToList();

            FrontPageModel frontPageView = 
                new FrontPageModel(lastFiveCustomerSurveys, lastFiveGenericSurveys, lastFiveCustomers, lastFiveUsers);
            return View(frontPageView);
        }
    }
}
