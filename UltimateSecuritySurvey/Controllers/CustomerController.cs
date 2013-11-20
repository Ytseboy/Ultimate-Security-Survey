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
            ViewBag.DeleteError = (TempData["Message"]) ?? string.Empty;
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
            return View("CreateEdit", new Customer());
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
            return View("CreateEdit",customer);
        }

        /// <summary>
        /// This method to edit the customer and save changes
        /// </summary>
        /// <param name="Customer">Customer class object from textboxes</param>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(Customer customer)
        {
            bool uniqueViolation = db.Customers.Any(x => x.email == customer.email
                                   && x.customerId != customer.customerId);


            if (ModelState.IsValid && !uniqueViolation)
            {

                //No Id => Add
                if (customer.customerId <= 0)
                {
                    db.Customers.Add(customer);
                }
                //Is Id => Update
                else
                {
                    db.Entry(customer).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Email must be unique!";
            return View(customer);
        }

        /// <summary>
        /// This method to open delete view and show the customer details to confirm delete
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
        /// and also check if customersurveys exist for the customer,
        /// if yes, then customer can't be deleted
        /// </summary>
        /// <param name="Customer">Primary id of Customer</param>

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            bool childExist = db.CustomerSurveys.Any(x => x.customerId == customer.customerId);

            if (!childExist)
            {
                db.Customers.Remove(customer);
                db.SaveChanges();
            }
            else
            {
                TempData["Message"] = "Cannot delete this Customer because related Customer Surveys exist!!";
            }

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}