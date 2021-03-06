using System;

namespace Assignment3.API.Services.Entities
{
    public class Courses{

        /// <summary>
        /// Database generated ID of the course
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// The name of the Course.
        /// Example: "Web Services"
        /// </summary>
        public string TemplateID { get; set; }

        /// <summary>
        /// The start date of the course
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The End date of the course
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Example "20151" -> spring 2015,
        /// "20152" -> summer 2015,
        /// "20153" -> fall 2015).
        /// </summary>
        public string Semester { get; set; }

        /// <summary>
        /// Maximum number of students in course
        /// </summary>
        public int? MaxStudents {get; set;}
    }
}