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
            ViewBag.surveyId = new SelectList(db.GenericSurveys, "surveyId", "title");
            ViewBag.observerUserId = new SelectList(db.UserAccounts, "userId", "firstName");
            ViewBag.supervisorUserId = new SelectList(db.UserAccounts, "userId", "firstName");
            return View();
        }

        //
        // POST: /CustomerSurvey/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerSurvey customersurvey)
        {
            if (ModelState.IsValid)
            {
                db.CustomerSurveys.Add(customersurvey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customerId = new SelectList(db.Customers, "customerId", "companyName", customersurvey.customerId);
            ViewBag.surveyId = new SelectList(db.GenericSurveys, "surveyId", "title", customersurvey.surveyId);
            ViewBag.observerUserId = new SelectList(db.UserAccounts, "userId", "firstName", customersurvey.observerUserId);
            ViewBag.supervisorUserId = new SelectList(db.UserAccounts, "userId", "firstName", customersurvey.supervisorUserId);
            return View(customersurvey);
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
            ViewBag.surveyId = new SelectList(db.GenericSurveys, "surveyId", "title", customersurvey.surveyId);
            ViewBag.observerUserId = new SelectList(db.UserAccounts, "userId", "firstName", customersurvey.observerUserId);
            ViewBag.supervisorUserId = new SelectList(db.UserAccounts, "userId", "firstName", customersurvey.supervisorUserId);
            return View(customersurvey);
        }

        //
        // POST: /CustomerSurvey/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerSurvey customersurvey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customersurvey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerId = new SelectList(db.Customers, "customerId", "companyName", customersurvey.customerId);
            ViewBag.surveyId = new SelectList(db.GenericSurveys, "surveyId", "title", customersurvey.surveyId);
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
            {
                return HttpNotFound();
            }
            return View(customersurvey);
        }

        //
        // POST: /CustomerSurvey/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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