using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UltimateSecuritySurvey.Models;

namespace UltimateSecuritySurvey.Controllers
{   
    /// <summary>
    /// To control Log In / Log out
    /// </summary>
    public class AccountController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        /// <summary>
        /// Redirects user to LogIn form
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Handles attempt to log in
        /// </summary>
        /// <param name="loginData">User name + password</param>
        /// <returns>Dashboar View</returns>
        [HttpPost]
        public ActionResult Login(string name, string password)
        {
            UserAccount userAccount = db.UserAccounts.FirstOrDefault(x => (x.userName == name || x.email == name) 
                                                            && x.password == password);
            if (userAccount != null)
            {
                FormsAuthentication.SetAuthCookie(userAccount.userName, true);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "User Name OR password is incorrect";
            return View(new { name = name, password = password});
        }

        /// <summary>
        /// To logOut, redirects user to Login form
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// To display surveys in date order this person is responsible
        /// </summary>
        /// <returns>My profile view</returns>
        public ActionResult MyProfile()
        {
            //Role & Login check --> delete when we have ok solution
            if (!Request.IsAuthenticated)
                return HttpNotFound();

            UserAccount user = db.UserAccounts.First(x => x.userName == User.Identity.Name);
            ViewBag.UserRole = user.isTeacher ? "Supervisor" : "Observer";

            //List of surveys (Newest first)
            IQueryable<CustomerSurvey> surveys = user.isTeacher ?
                        db.CustomerSurveys.Where(x => x.supervisorUserId == user.userId) :
                        db.CustomerSurveys.Where(x => x.observerUserId == user.userId);

            ViewBag.Surveys = surveys.OrderByDescending(z => z.startDate).ThenByDescending(w => w.endDate).ToList();

            //Avatar =)))
            Random rnd = new Random();
            int next = rnd.Next(1, 6);
            ViewBag.AvaUrl = String.Format("~/Content/imgs/ava/{0}.jpg", next);

            return View(user);
        }
    }
}
