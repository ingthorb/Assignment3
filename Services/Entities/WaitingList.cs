using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment3.API.Services.Entities
{
    public class WaitingList
    {

            
        /// <summary>
        /// Database generated number for the student in the WaitingList
        /// </summary>
        /// <returns></returns>
        [Key]
        public int Number { get; set; }

        public int CourseID { get; set; }

        /// <summary>
        /// The Students social seciurity number 
        /// </summary>
        /// <returns></returns>
        public long SSN { get; set; }

    }
}