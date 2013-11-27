using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UltimateSecuritySurvey.Models
{
    public class GenericSurveyQuestion
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool Assigned { get; set; }
    }
}