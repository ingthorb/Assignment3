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

        public int Active {get; set;  }
    }
}