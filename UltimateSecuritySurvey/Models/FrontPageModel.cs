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
        /// This is the constructor for the lists
        /// </summary>
        /// <param name="questions">List of questions</param>
        /// <param name="categories">List of categories</param>
        
        public FrontPageModel(List<Question> questions, List<QuestionCategory> categories) 
        {
            this.questions = questions;
            this.categories = categories;
        }
        /// <summary>
        /// Get Set for Questions list
        /// </summary>
        public List<Question> questions { get; set; }
        /// <summary>
        /// Get Set for Categories list
        /// </summary>
        public List<QuestionCategory> categories { get; set; }
    }
}