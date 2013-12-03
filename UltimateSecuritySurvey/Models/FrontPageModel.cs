using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UltimateSecuritySurvey.Models
{
    /// <summary>
    /// This is our front page model for the question list and the category list
    /// </summary>
    public class FrontPageModel
    {
        /// <summary>
        /// The constructor for FrontPage Model
        /// </summary>
        /// <param name="surveys">List of surveys</param>
        /// <param name="customers">List of customers</param>
        /// <param name="users">List of users</param>
        public FrontPageModel(List<CustomerSurvey> customerSurveys, List<Customer> customers, List<UserAccount> users) 
        {
            this.customerSurveys = customerSurveys;
            this.customers = customers;
            this.users = users;
        }

        public List<CustomerSurvey> customerSurveys { get; set; }

        /// <summary>
        /// Get set for Customer List
        /// </summary>
        public List<Customer> customers { get; set; }

        /// <summary>
        /// Get set for User List
        /// </summary>
        public List<UserAccount> users { get; set; }
    }
}