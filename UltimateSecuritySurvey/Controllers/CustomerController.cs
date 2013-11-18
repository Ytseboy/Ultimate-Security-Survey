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
    /// This controller to Display Customers and to manage them, CRUD
    /// </summary>
    public class CustomerController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();

        /// <summary>
        /// This method to list the customers on the index page
        /// </summary>

        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        /// <summary>
        /// This method to find a customer with id from the database
        /// </summary>
        /// <param name="id">Primary id of Customer</param>
        //

        public ActionResult Details(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        /// <summary>
        /// This method to go to the create view
        /// </summary>

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// This method to go add a new customer to the database and save it
        /// </summary>
        /// <param name="Customer">Customer class object from textboxes</param>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        /// <summary>
        /// This method to go to edit view, gets also the customer details from db
        /// </summary>
        /// <param name="Customer">Customer class object</param>

        public ActionResult Edit(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        /// <summary>
        /// This method to edit the customer and save changes
        /// </summary>
        /// <param name="Customer">Customer class object from textboxes</param>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        /// <summary>
        /// This method to open delete view and show the customer details3
        /// </summary>
        /// <param name="Customer">Primary id of Customer</param>

        public ActionResult Delete(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        /// <summary>
        /// This method to delete the customer from database
        /// </summary>
        /// <param name="Customer">Primary id of Customer</param>

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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