using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment3.API.Services.Entities
{
    public class WaitingList
    {

            
        /// <summary>
        /// Database generated number for the student in the WaitingList
        /// </summary>
        [Key]
        public int Number { get; set; }

        /// <summary>
        /// The id of the Course
        /// </summary>
        public int CourseID { get; set; }

        /// <summary>
        /// The Students social seciurity number 
        /// </summary>
        public long SSN { get; set; }

    }
}