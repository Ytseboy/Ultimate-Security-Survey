using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UltimateSecuritySurvey.Models;

namespace UltimateSecuritySurvey.Controllers
{
    [Authorize]
    public class CustomerAnswerController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        //
        // GET: /CustomerAnswer/

        public ActionResult Index(int id = 0)
        {
            CustomerSurvey customerSurvey = db.CustomerSurveys.Find(id);
            if (customerSurvey == null)
                return HttpNotFound();

            List<CustomerAnswer> answerList;

            //Teacher see only created by student answers
            answerList = customerSurvey.CustomerAnswers.ToList();
            ViewBag.SurveyTitle = customerSurvey.customerSurveyTitle;
            return View("IndexTeacher", answerList);

            //LATER TO DO = STUDENT See all the questions included in generic survey     
        }

        //
        // GET: /CustomerAnswer/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomerAnswer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerAnswer customeranswer)
        {
            if (ModelState.IsValid)
            {
                db.CustomerAnswers.Add(customeranswer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customeranswer);
        }

        //
        // GET: /CustomerAnswer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomerAnswer customeranswer = db.CustomerAnswers.Find(id);
            if (customeranswer == null)
            {
                return HttpNotFound();
            }
            return View(customeranswer);
        }

        //
        // POST: /CustomerAnswer/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerAnswer customeranswer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customeranswer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customeranswer);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}