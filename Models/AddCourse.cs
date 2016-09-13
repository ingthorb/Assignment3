using System;
using System.ComponentModel.DataAnnotations; 

namespace Assignment3.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddCourse
    {

         /// <summary>
         /// The name of the Course.
         /// Example: "Web Services"
         /// </summary>
         [Required]
        public string TemplateID {get;set;}

        /// <summary>
        /// The start date of the Course
        /// </summary>
        /// <returns></returns>
        [Required]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The end date of the course
        /// </summary>
        /// <returns></returns>
        [Required]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Example "20151" -> spring 2015, 
        /// "20152" -> summer 2015,
        /// "20153" -> fall 2015). 
        /// </summary>
        [Required]
        public string Semester {get; set;}

        /// <summary>
        /// Max number of students
        /// </summary>
        /// <returns></returns>
        [Required]
        public int?  MaxStudents { get; set; }
    }
}
