namespace Assignment3.API.Services.Entities
{
    public class StudentsInCourse{

        
        /// <summary>
        /// Database generated id for the student in course connection
        /// </summary>
        /// <returns></returns>
        public int ID { get; set; }

        /// <summary>
        /// The id of the course
        /// </summary>
        /// <returns></returns>
        public int CourseID { get; set; }
        
        /// <summary>
        /// The Students social seciurity number 
        /// </summary>
        /// <returns></returns>
        public long SSN { get; set; }

        /// <summary>
        /// Tells us if the student is active in the Course 
        /// 1 if the student is active 0 if the student is not active
        /// </summary>
        public int Active {get; set;  }
    }
}