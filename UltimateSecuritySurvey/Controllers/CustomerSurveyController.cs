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
    /// <summary>
    /// Controller to handle customers surveys
    /// </summary>
    public class CustomerSurveyController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        //
        // GET: /CustomerSurvey/

        public ActionResult Index()
        {
            var customersurveys = db.CustomerSurveys.Include(c => c.Customer).Include(c => c.GenericSurvey).Include(c => c.UserAccount).Include(c => c.UserAccount1);
            return View(customersurveys.ToList());
        }

        //
        // GET: /CustomerSurvey/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomerSurvey customersurvey = db.CustomerSurveys.Find(id);
            if (customersurvey == null)
            {
                return HttpNotFound();
            }
            return View(customersurvey);
        }

        //
        // GET: /CustomerSurvey/Create

        public ActionResult Create()
        {
            ViewBag.customerId = new SelectList(db.Customers, "customerId", "companyName");
            ViewBag.baseSurveyId = new SelectList(db.GenericSurveys, "surveyId", "title");
            ViewBag.observerUserId = new SelectList(db.UserAccounts, "userId", "firstName");
            ViewBag.supervisorUserId = new SelectList(db.UserAccounts, "userId", "firstName");
            return View("CreateEdit", new CustomerSurvey());
        }

        //
        // GET: /CustomerSurvey/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomerSurvey customersurvey = db.CustomerSurveys.Find(id);
            if (customersurvey == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerId = new SelectList(db.Customers, "customerId", "companyName", customersurvey.customerId);
            ViewBag.baseSurveyId = new SelectList(db.GenericSurveys, "surveyId", "title", customersurvey.baseSurveyId);
            ViewBag.observerUserId = new SelectList(db.UserAccounts, "userId", "firstName", customersurvey.observerUserId);
            ViewBag.supervisorUserId = new SelectList(db.UserAccounts, "userId", "firstName", customersurvey.supervisorUserId);
            return View("CreateEdit", customersurvey);
        }

        //
        // POST: /CustomerSurvey/Edit/5

        [HttpPost]
        public ActionResult CreateEdit(CustomerSurvey customersurvey)
        {
            if (ModelState.IsValid)
            {
                //If No ID => ADD
                if (customersurvey.surveyId <= 0)
                {
                    db.CustomerSurveys.Add(customersurvey);
                }
                //IF there is ID => Update
                else
                {
                    db.Entry(customersurvey).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerId = new SelectList(db.Customers, "customerId", "companyName", customersurvey.customerId);
            ViewBag.baseSurveyId = new SelectList(db.GenericSurveys, "surveyId", "title", customersurvey.baseSurveyId);
            ViewBag.observerUserId = new SelectList(db.UserAccounts, "userId", "firstName", customersurvey.observerUserId);
            ViewBag.supervisorUserId = new SelectList(db.UserAccounts, "userId", "firstName", customersurvey.supervisorUserId);
            return View(customersurvey);
        }

        //
        // GET: /CustomerSurvey/Delete/5
        public ActionResult Delete(int id = 0)
        {
            CustomerSurvey customersurvey = db.CustomerSurveys.Find(id);
            if (customersurvey == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return PartialView();

            return View(customersurvey);
        }

        //
        // POST: /CustomerSurvey/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerSurvey customersurvey = db.CustomerSurveys.Find(id);
            db.CustomerSurveys.Remove(customersurvey);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}