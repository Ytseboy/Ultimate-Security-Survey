using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UltimateSecuritySurvey.Models
{
    public class FrontPageModel
    {
        SecuritySurveyEntities db = new SecuritySurveyEntities();

        public FrontPageModel() 
        {
            this.myQuestion = db.Questions.Find("q1");
        }

        public Question myQuestion { get; set; }
    }
}