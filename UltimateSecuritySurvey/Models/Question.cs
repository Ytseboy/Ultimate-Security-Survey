//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UltimateSecuritySurvey.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Question
    {
        public Question()
        {
            this.AnswerOptions = new HashSet<AnswerOption>();
            this.CustomerAnswers = new HashSet<CustomerAnswer>();
            this.GenericCountermeasures = new HashSet<GenericCountermeasure>();
            this.GenericSurveys = new HashSet<GenericSurvey>();
        }
    
        public int questionId { get; set; }

        [DisplayName("Category Id")]
        [Required(ErrorMessage = "Category Id is required!")]
        public int categoryId { get; set; }

        [DisplayName("Question Type Id")]
        [Required(ErrorMessage = "Question Type Id is required!")]
        public int questionTypeId { get; set; }

        [DisplayName("Question Main")]
        [Required(ErrorMessage = "Question Main is required!")]
        [StringLength(8000, ErrorMessage = "Text maximum length is 8000 characters.")]
        [DataType(DataType.MultilineText)]
        public string questionTextMain { get; set; }

        [DisplayName("Question Extra")]
        [StringLength(1000, ErrorMessage = "Text maximum length is 1000 characters.")]
        [DataType(DataType.MultilineText)]
        public string questionTextExtra { get; set; }

        [DisplayName("Base Level 2 Requirement")]
        [Required(ErrorMessage = "Base Level 2 requirements are required!")]
        [StringLength(8000, ErrorMessage = "Text maximum length is 8000 characters.")]
        [DataType(DataType.MultilineText)]
        public string baseLevel2RequirementText { get; set; }

        [DisplayName("Additional info")]
        [StringLength(8000, ErrorMessage = "Text maximum length is 8000 characters.")]
        [DataType(DataType.MultilineText)]
        public string additionalInfo { get; set; }

        [DisplayName("Additional note")]
        [StringLength(8000, ErrorMessage = "Text maximum length is 8000 characters.")]
        [DataType(DataType.MultilineText)]
        public string additionalNote { get; set; }
    
        public virtual ICollection<AnswerOption> AnswerOptions { get; set; }
        public virtual ICollection<CustomerAnswer> CustomerAnswers { get; set; }
        public virtual ICollection<GenericCountermeasure> GenericCountermeasures { get; set; }
        public virtual QuestionCategory QuestionCategory { get; set; }
        public virtual QuestionType QuestionType { get; set; }
        public virtual ICollection<GenericSurvey> GenericSurveys { get; set; }
    }
}
