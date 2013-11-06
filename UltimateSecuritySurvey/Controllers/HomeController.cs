using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UltimateSecuritySurvey.Models;
namespace UltimateSecuritySurvey.Controllers
{
    public class HomeController : Controller
    {
   
        //
        // GET: /Home/

        public ActionResult Index()
        {
            FrontPageModel question = new FrontPageModel();
            return View(question);
        }

    }
}
