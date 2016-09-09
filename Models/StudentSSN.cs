using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment3.API.Models
{
    /// <summary>
    /// The social security number of the student we want to add to a course
    /// </summary>
    public class StudentSSN
    {
        /// <summary>
        /// The students social security number
        /// </summary>
        /// <returns></returns>
        [Required]
        public long SSN {get;set;}
    }
}
