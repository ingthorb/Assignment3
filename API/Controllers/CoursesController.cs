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
         /// <returns>List of courses on that semester</returns>
         [HttpGet]
         public IActionResult GetCoursesOnSemester(string semester)
         {
           return Ok(_service.GetCoursesBySemester(semester));  
         }


        /// <summary>
        /// The funtion returns information about one course with spesific id
        /// </summary>
        /// <param name="id">The parameter of the couse we want to return</param>
        /// <returns>Returns detailed course object</returns>
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
         /// <returns>List of students in a course</returns>
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
        /// Should allow the client of the API to modify the given course instance.
        /// The properties which should be mutable are StartDate and EndDate, 
        /// others (CourseID and Semester) should be immutable.
        /// </summary>
        /// <param name="id">The id of the course we want to update</param>
        /// <param name="coursedt">The course updated information we take in from the body</param>
        /// <returns></returns>
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
                    //Virkar ekki
                    //return InternalServerError(ex);
                    return BadRequest(); 
            }
           }
           else
           {
               return BadRequest();
           }
           return NoContent();
         }

        [HttpPost]
       // [Route("api/courses"),Name="AddCourse")]
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
        /// <returns>Returns bad request if we could not add the studetn <summary>
        /// or ok(); if we could add the student
        /// </summary>
         [HttpPost]
        [Route("{id:int}/students", Name="AddStudentToCourse")]
        public IActionResult AddStudentToCourse(int _id,[FromBody] StudentSSN Student) 
        {
            Console.WriteLine("AddStudentToCoursecontroller");
            StudentSSN addStudent = new StudentSSN();
            if(ModelState.IsValid)
            {
                try
                {
                    addStudent = _service.AddStudentToCourse(_id,Student);
                }
                catch (StudentIsInCourseException )
                {
                    return BadRequest();
                }
                catch (FailedToSaveToDatabaseException )
                {
                    //Virkar ekki
                    //return InternalServerError(ex);
                    return BadRequest();
                }
                catch (StudentNonExistException )
                {
                    return BadRequest();
                }
                
                
            }
            else
            {
                Console.WriteLine("INVALID");
                return BadRequest();
            }
            Console.WriteLine("ISVALID");
            var location = Url.Link("AddStudentToCourse", new { id = _id });  
            return Created(location,Student);
       }

        /// <summary>
        /// Removes the course by given id
        /// </summary>
        /// <param name="id">The id of the course we want to remove</param>
        /// <returns>Returns no content if the course was succesfully deleted 
        /// and 404 if the course was not found in the db
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
                //Virkar ekki
                //return InternalServerError(ex);
                return BadRequest();
            }
            return NoContent();
        }
    }
}