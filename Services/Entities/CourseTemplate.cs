namespace Assignment3.API.Services.Entities
{
    public class CourseTemplate{

        
        /// <summary>
        /// The id of the Course template. Database generated. 
        /// </summary>
        public int ID { get; set; } 

        /// <summary>
        /// The ID of the Course.
        /// Example: "T-514-VEFT"
        /// </summary>
        public string TemplateID { get; set; }

        /// <summary>
        /// The name of the Course.
        /// Example: "Web Services" 
        /// </summary>
        public string Name { get; set; }
    }
}