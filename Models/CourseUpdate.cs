using System;
using System.ComponentModel.DataAnnotations; 

namespace Assignment3.API.Models
{
    /// <summary>
    /// The cours values that can be uppdated
    /// </summary>
    public class CourseUpdate
    {
        /// <summary>
        /// The start Date of the course
        /// </summary>
        [Required]
        public string StartDate { get; set; }
 
        /// <summary>
        /// The end date of the course
        /// </summary>
        [Required]
        public string EndDate { get; set; }
    }
}