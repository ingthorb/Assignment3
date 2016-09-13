using System;

namespace Assignment3.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class StudentsInCourses
    {

      /// <summary>
      /// The stuedents name
      /// </summary>
      public int CourseID {get; set;}
      
      /// <summary>
      /// The students social security number
      /// </summary>
      public long SSN {get;set;}

      /// <summary>
      /// Tells us if the student is active in the Course 
      /// 1 if the student is active 0 if the student is not active
      /// </summary>
      public int Active { get; set; }

    }
}
