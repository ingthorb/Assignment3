using System;

namespace Assignment3.API.Models
{
    /// <summary>
    /// Short summary of the course 
    /// </summary>
    public class CourseDTOLite
    {
      /// <summary>
      /// Database generated ID of the course 
      /// </summary>
      public int ID {get; set;}

      /// <summary>
      /// Database generated ID of the course 
      /// </summary>
      public string Name {get; set;}

      /// <summary>
      /// The name of the Course.
      /// Example: "Web Services"
      /// </summary>
      public string Semester {get;set;}

      /// <summary>
      /// Count of students in class
      /// </summary>
      public int StudentCount { get; set; }
    }
}