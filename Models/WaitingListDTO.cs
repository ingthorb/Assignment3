using System;

namespace Assignment3.API.Models
{
    public class WaitingListDTO
    {

        public int CourseID { get; set; }

        /// <summary>
        /// The Students social seciurity number 
        /// </summary>
        /// <returns></returns>
        public long SSN { get; set; }

    }
}