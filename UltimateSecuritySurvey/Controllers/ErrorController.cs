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
    [Authorize]
    public class ErrorController : Controller
    {
        /// <summary>
        /// 404 code error
        /// </summary>
        /// <returns>404 custom view</returns>
       
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}
