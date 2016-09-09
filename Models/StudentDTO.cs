using System;

namespace Assignment3.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class StudentDTO
    {

      /// <summary>
      /// The id of the stuedent 
      /// </summary>
      public int ID {get;set;}
      /// <summary>
      /// The students social security number
      /// </summary>
      public long SSN {get;set;}

      /// <summary>
      /// The stuedents name
      /// </summary>
      public string Name {get; set;}
    }
}
