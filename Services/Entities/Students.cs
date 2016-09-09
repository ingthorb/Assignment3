namespace Assignment3.API.Services.Entities
{
    public class Students{

        /// <summary>
        /// The id of the student 
        /// </summary>
        /// <returns></returns>
        public int ID { get; set; }

        /// <summary>
        /// The social security number of the student
        /// </summary>
        /// <returns></returns>
        public int SSN { get; set; }

        /// <summary>
        /// The students Name
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
    }
}