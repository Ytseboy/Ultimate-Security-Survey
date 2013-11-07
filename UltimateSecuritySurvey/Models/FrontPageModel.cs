using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UltimateSecuritySurvey.Models
{
    public class FrontPageModel
    {
        public FrontPageModel() { }
        public FrontPageModel(List<Question> questions, List<QuestionCategory> categories) 
        {
            this.questions = questions;
            this.categories = categories;
        }
        public List<Question> questions { get; set; }
        public List<QuestionCategory> categories { get; set; }
    }
}