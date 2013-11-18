using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UltimateSecuritySurvey.Controllers
{
    /// <summary>
    /// This Controller contains actions for possible errors
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// 404 code error
        /// </summary>
        /// <returns>404 custom view</returns>
        [HttpPost]
        public ActionResult PageNotFound()
        {
            return View();
        }

        /// <summary>
        /// In case Student will try to look inside of Teacher only pages
        /// </summary>
        /// <returns>custom error view</returns>
        [HttpPost]
        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}
