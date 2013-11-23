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
            else
            {
                ModelState.AddModelError("", "User Name OR password is incorrect");
            }
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
    }
}
