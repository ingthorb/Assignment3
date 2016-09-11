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

      public int Active { get; set; }

    }
}
