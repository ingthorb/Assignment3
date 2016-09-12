using System;

namespace Assignment3.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CoursesDTO
    {
      /// <summary>
      /// Database generated ID of the course 
      /// </summary>
      public int ID {get; set;}

      /// <summary>
      /// The name of the Course.
      /// Example: "Web Services"
      /// </summary>
      public string TemplateID {get;set;}

        /// <summary>
        /// The start date of the Course
        /// </summary>
        /// <returns></returns>
        public string StartDate { get; set; }

        /// <summary>
        /// The end date of the course
        /// </summary>
        /// <returns></returns>
        public string EndDate { get; set; }
       /// <summary>
       /// Example "20151" -> spring 2015, 
       /// "20152" -> summer 2015,
       /// "20153" -> fall 2015). 
       /// </summary>
       public string Semester {get; set;}

        /// <summary>
        /// Max number of students in course
        /// </summary>
        public int? MaxStudents {get; set;}
    }
}
