
using System;
using System.Collections.Generic;

namespace Assignment3.API.Models
{
    /// <summary>
    /// Detailed information about the course
    /// </summary>
    public class CourseDetailed
    {
      /// <summary>
      /// Database generated ID of the course
      /// </summary>
      public int ID {get; set;}
           
      /// <summary>
      /// The name of the Course.
      /// Example: "Web Services"
      /// </summary>
      public string Name {get; set;}
 
      /// <summary>
      /// The ID of the Course.
      /// Example: ""
      /// </summary>
      public string TemplateID {get;set;}
 
      /// <summary>
      /// Example "20151" -> spring 2015,
      /// "20152" -> summer 2015,
      /// "20153" -> fall 2015).
      /// </summary>
      public string Semester {get; set;}
 
        /// <summary>
        /// The start date of the course
        /// </summary>
        public string StartDate { get; set; }
 
        /// <summary>
        /// The End date of the course
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// List of Student in this course 
        /// </summary>
        public List<StudentDTO> listOfStudents { get;set;}
    }
}