using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UltimateSecuritySurvey.Controllers
{
    /// <summary>
    /// This controller doesn't perform any task
    /// I simply returns a View where user can read
    /// About application
    /// </summary>
    [Authorize]
    public class AboutController : Controller
    {
        /// <summary>
        /// About application
        /// </summary>
        /// <returns>Index view</returns>
        //
        // GET: /About/
        public ActionResult Index()
        {
            return View();
        }

    }
}
