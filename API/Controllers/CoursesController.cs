using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assignment3.API.Models;
using Assignment3.API.Services;
using Assignment3.API.Services.Entities;

namespace Assignment3.API.Controllers {
    [Route("api/courses")]
    public class CoursesController : Controller {
        private readonly ICoursesService _service;


        public CoursesController(ICoursesService service) {
            _service = service;
        }

         /// <summary>
         /// Gets couse by semester.
         /// If no semester is provided in the query (i.e. /api/courses), the current semester should be used
         /// </summary>
         /// <param name="semester"> 
         /// example: "20151" -> spring 2015, "20152" -> summer 2015, "20153" -> fall 2015
         /// </param>
         /// <returns>Ok and list of courses on that semester</returns>
         [HttpGet]
         public IActionResult GetCoursesOnSemester(string semester)
         {
           return Ok(_service.GetCoursesBySemester(semester));  
         }

        /// <summary>
        /// The funtion returns information about one course with spesific id
        /// </summary>
        /// <param name="id">The parameter of the couse we want to return</param>
        /// <returns>404 if the course is not found. Ok and the course if the course if the course is found.</returns>
        [HttpGet]
        [Route("{id:int}",Name="GetCourseByID")]
         public IActionResult GetCourseByID(int id){
             Console.WriteLine("GetCourseByID");
             CourseDetailed course =  new CourseDetailed();
             try
             {
                course = _service.GetCourseByID(id);     
             }
             catch (AppObjectNotFoundException )
             {
                 return NotFound();
             }
            return Ok(course);
         }
 
         /// <summary>
         /// Should get the list of students by course id 
         /// </summary>
         /// <param name=""{id:int}/students"">the id of the cours we want to get student list from</param>
         /// <param name="id"></param>
         /// <returns>Ok with List of students in a course if we are able to get the list. 404 if we do not find the course. </returns>
         [HttpGet]
         [Route("{id:int}/students", Name="GetStudents")]
         public IActionResult GetListOfStudentsByCourseId(int id) {

             List<StudentDTO> students = new List<StudentDTO>();
             try
             {
                students = _service.GetListOfStudentsByCourseId(id);
             }
             catch (AppObjectNotFoundException)
             {
                 return NotFound();
             }
             return Ok(students);
         }

        /// <summary>
        /// Waiting list for courses
        /// </summary>
        /// <param name="id">The id of the course that we want to get the waiting list for</param>
        /// <returns>Returns 404 if the course is not found. Ok if we are able to add the student to the waiting list </returns>
        [HttpGet]
        [Route("{id:int}/waitinglist", Name="GetWaitingList")]
        public IActionResult GetWaitingList(int id) {
            List<StudentDTO> students = new List<StudentDTO>();
            try
            {
                students = _service.GetWaitingList(id);
            }
            catch (AppObjectNotFoundException)
            {
                return NotFound();
            }
            return Ok(students);
         }

        /// <summary>
        /// Should allow the client of the API to modify the given course instance.
        /// The properties which should be mutable are StartDate and EndDate, 
        /// others (CourseID and Semester) should be immutable.
        /// </summary>
        /// <param name="id">The id of the course we want to update</param>
        /// <param name="coursedt">The course updated information we take in from the body</param>
        /// <returns>BadRequest if the model state is not valid. We return 404 if the course does not exist 
         /// in the database. We return 500 if we are not able to save to database and no content when we are able to update the course. 
         /// </returns>
        [HttpPut]
        [Route("{id:int}")]
         public IActionResult UpdateCourse(int id, [FromBody] CourseUpdate coursedt)
        {
    
            CoursesDTO course = new CoursesDTO();
            if(ModelState.IsValid)
            {
                try
                {
                    course = _service.UpdateCourse(id,coursedt);  
                }
                catch(AppObjectNotFoundException )
                {
                    return NotFound();
                }
                catch(FailedToSaveToDatabaseException)
                {
                    return StatusCode(500);
                }
            }
            else
            {
                return BadRequest();
            }
            return NoContent();
        }

        /// <summary>
        /// Creates a course
        /// </summary>
        /// <param name="courses">Informations needed to add course retrived from the Body</param>
        /// <returns>Bad request if the model state is not valid. Id and location header and 201 if we could create the course</returns>
        [HttpPost]
        public IActionResult CreateCourse([FromBody] AddCourse courses) 
        {
           Courses course = new Courses(); 
           
           if(ModelState.IsValid)
           {
                course = _service.CreateCourse(courses);
           }
           else
           {
               return BadRequest();
           }
           var location = Url.Link("GetCourseByID", new { id =  course.ID });  
           return Created(location,course);
        }

        /// <summary>
        /// Should add student to course
        /// </summary>
        /// <param name="id">The id of the course</param>
        /// <param name="Student">The model object StudentSSN wich contains the SSN of the student</param>
        /// <returns>If the model state is not valid we return bad request <summary>
        /// If the course or student does not exist we return 404. If it fails to save to database we return 500.
        /// If the maximum number of students in course is reched we return 412. If the student is alredy in the 
        /// course we return 412. If we are able to add to the waitingList we return Ok().
        /// </summary>
        [HttpPost]
        [Route("{id:int}/students", Name="AddStudentToCourse")]
        public IActionResult AddStudentToCourse(int id,[FromBody] StudentSSN Student) 
        {
            Console.WriteLine("AddStudentToCoursecontroller");
            StudentSSN addStudent = new StudentSSN();
            if(ModelState.IsValid)
            {
                try
                {
                    addStudent = _service.AddStudentToCourse(id,Student);
                }
                catch (StudentIsInCourseException )
                {
                    return StatusCode(412);
                }
                catch (MaxNumberOfStudentsException)
                {
                    return StatusCode(412);
                }
                catch (FailedToSaveToDatabaseException )
                {
                    return StatusCode(500);
                }
                catch (StudentNonExistException )
                {
                    return NotFound();
                }
                catch (AppObjectNotFoundException)
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }

            var location = Url.Link("AddStudentToCourse", new { id = id });  
            return Created(location,Student);
       }

        /// <summary>
        /// Adds a student to a waiting list for spesific course
        /// </summary>
        /// <param name="id">The id of the course </param>
        /// <param name="Student">The SSN of the student we are adding to the waiting list</param>
        /// <returns>If the course or student does not exist we return 404. If the student is alredy in 
        /// the course or on the waiting list we return 412. If the model state is not valid we return bad request
        /// If we are able to add to the waitingList we return Ok()</returns>
        [HttpPost]
        [Route("{id:int}/waitinglist", Name="AddToWaitingList")]
        public IActionResult AddToWaitingList(int id,[FromBody] StudentSSN Student) 
        {
            StudentSSN addStudent = new StudentSSN();
            if(ModelState.IsValid)
            {
                try
                {
                    addStudent = _service.AddToWaitingList(id,Student);
                }
                catch (AppObjectNotFoundException )
                {
                    return NotFound();
                }
                catch(StudentNonExistException)
                {
                    return NotFound();
                }
                catch(StudentIsInCourseException)
                {
                    return StatusCode(412);
                }
                catch(StudentOnWaitingListException)
                {
                    return StatusCode(412);
                }
            }
            else
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Th
        /// </summary>
        /// <param name="id"></param>
        /// <param name="SSN">The social security number of the student to delete</param>
        /// <returns>Nocontent if we succesfully deleted the student, 404 if we could not find the course to
        /// delete from. If it fails to save to database we return 500
        ///</returns>
        [HttpDelete]
        [Route("{id:int}/students/{ssn:long}")]
        public IActionResult DeleteCourse(int id, long SSN) {
            StudentSSN studentdelete = new StudentSSN();
            try
            {
                studentdelete = _service.DeleteStudent(id, SSN);
            }
            catch (AppObjectNotFoundException )
            {
                return NotFound();
            }
            catch(FailedToSaveToDatabaseException )
            {
                return StatusCode(500);
            }
            return NoContent();
        }

        /// <summary>
        /// Removes the course by given id
        /// </summary>
        /// <param name="id">The id of the course we want to remove</param>
        /// <returns>Returns no content if the course was succesfully deleted 
        /// and 404 if the course was not found in the db. If it fails to save to database we return 500
        /// </returns>
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteCourse(int id) {
            CoursesDTO courseDeleted = new CoursesDTO();
            try
            {
                courseDeleted = _service.DeleteCourse(id);
            }
            catch (AppObjectNotFoundException )
            {
                return NotFound();
            }
            catch(FailedToSaveToDatabaseException )
            {
                return StatusCode(500);
            }
            return NoContent();
        }
    }
}
