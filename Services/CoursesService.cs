using System;
using System.Collections.Generic;
using Assignment3.API.Models;
using Assignment3.API.Services.Entities;
using System.Linq;

namespace Assignment3.API.Services
{
    public class CoursesService : ICoursesService
    {
        //ORDER: GET, PUT, POST, DELETE

        //Console.WriteLine("Course SErvice");
        private readonly AppDataContext _db;
        public CoursesService(AppDataContext db){
            _db = db;
        }

        /// <summary>
        /// Returns a list of courses, if no semaster is passed in we add the current semaster
        /// </summary>
        /// <param name="semester">
        /// Example "20151" -> spring 2015,
        /// "20152" -> summer 2015,
        /// "20153" -> fall 2015).
        /// </param>
        /// <returns>List<CourseDTOLite></returns>
        public List<CourseDTOLite> GetCoursesBySemester(string semester ){
            
         Console.WriteLine("GetCoursesBySemester");
            if(semester == null)
            {
                semester = "20163";
            }
            var result = (from c in _db.Courses
                join ct in _db.CourseTemplate on c.TemplateID equals ct.TemplateID 
                where c.Semester == semester
                select new CourseDTOLite{
                    ID  = c.ID,
                    Name = ct.Name,
                    Semester = c.Semester
                }).ToList();

                foreach(var it in result)
                {
                    int count = GetListOfStudentsByCourseId(it.ID).Count;
                    it.StudentCount = count;
                }
           return result;
         }  



        /// <summary>
        /// Returns a detailed information about a couse 
        /// </summary>
        /// <param name="ID">The id of the couse we want to get information about</param>
        /// <returns>Returns CourseDetailed</returns>
       public CourseDetailed GetCourseByID(int ID){  
         Console.WriteLine("GetCourseByID");

            //Þurfti að breyta þar sem að kalla inní getlist fallið var að fokka upp
            //Ef thad tharf ekki ad checka hvort course se til i addstudent tha ma kalla annars ekki

            var listOfStudents2 =  (from x in _db.StudentsInCourses
            join ct in _db.Students on x.SSN equals ct.SSN 
            where x.CourseID == ID
            select new StudentDTO{
                 ID = ct.ID,
                 SSN = ct.SSN,
                 Name = ct.Name
             }).ToList();


             var course = (from x in _db.Courses
             join ct in _db.CourseTemplate on x.TemplateID equals ct.TemplateID
             where x.ID == ID
              select new CourseDetailed{
                  ID = x.ID,
                  Name = ct.Name,
                  TemplateID = x.TemplateID,
                  Semester = x.Semester,
                  StartDate  = x.StartDate,
                  EndDate = x.EndDate,
                  listOfStudents = listOfStudents2
              }).SingleOrDefault();

              if(course == null)
              {
                  Console.WriteLine("GetCourseByID Exception");
                  throw new AppObjectNotFoundException();
              }


              return course;
         }
           /// <summary>
        /// Function that returns a list of all students in a spesific course
        /// </summary>
        /// <param name="id">The id of the couse</param>
        /// <returns>List<StudentDTO></returns>
        public List<StudentDTO> GetListOfStudentsByCourseId(int id){
             Console.WriteLine("GetListOfStudentsByCourseId");
            //Only list active users not that have been deleted(SaveChanges)

            //Gives StackOverFlowException
            //var checkCourse = GetCourseByID(id);

            //Það að kalla í þetta fall úr GetCourseByID var að fokka upp checkinu
            //Ef við thurfum ekki að tjekka hvort þessi course se til þa er hægt ad kalla i thad
            var course = (from c in _db.Courses
            where c.ID == id
            select c).SingleOrDefault();

            if(course == null)
            {
                throw new AppObjectNotFoundException();
            }
            //Erum að skila tomum lista ef thad er enginn skradur getum breytt
            var listOfStudents =  (from x in _db.StudentsInCourses
            join ct in _db.Students on x.SSN equals ct.SSN 
            where x.CourseID == id
            select new StudentDTO{
                 ID = ct.ID,
                 SSN = ct.SSN,
                 Name = ct.Name
             }).ToList();
             Console.WriteLine("SSN");
             Console.WriteLine(listOfStudents[1].SSN);
             return listOfStudents;
         }

         /// <summary>
         /// Allows us to update the startdate and endate of course
         /// </summary>
         /// <param name="id">The id of the course we want to update</param>
         /// <param name="coursedt">instance of CourseUpdate that we can get startdate and enddate from</param>
         /// <returns>boolean value if we could not update couse</returns>
         public CoursesDTO UpdateCourse(int id,CourseUpdate coursedt)
         {
             var courseToUpdate = (from x in _db.Courses
             where x.ID == id
             select new CoursesDTO{
                 ID = x.ID,
                 TemplateID = x.TemplateID,
                 Semester = x.Semester,
                 StartDate = x.StartDate,
                 EndDate = x.EndDate
             }).SingleOrDefault();
             
             if(courseToUpdate == null)
             {
                 throw new AppObjectNotFoundException();
             }
             else
             {
                 Console.WriteLine("WE GO INTO UPDATE");    
                 courseToUpdate.StartDate = coursedt.StartDate;
                 courseToUpdate.EndDate   = coursedt.EndDate;
                 try
                 {
                     //Fer herna inn en vill ekki updateast
                     Console.WriteLine("WE GO INTO Save"); 
                     _db.SaveChanges();
                 }
                 catch (System.Exception)
                 {
                    throw new FailedToSaveToDatabaseException();
                 }
             }
             return courseToUpdate;
         }
 

       public AddCourse CreateCourse(AddCourse course) 
       {
          // Fæ error á að ekkert ID sé??
          /* var newCourse = new Entities.Course {
               CourseID = course.CourseID,
               StartDate = course.StartDate,
               EndDate = course.EndDate,
               Semester = course.Semester,
               MaxStudents = course.MaxStudents
           };
            _db.Courses.Add(newCourse);
           return course;*/
           return course;
       }

        /// <summary>
        /// Adds student to the table StudentsInCourse 
        /// </summary>
        /// <param name="id">The id of the couse we are going to add in</param>
        /// <param name="student">The Student of type StudentSSN containing the students SSN </param>
        /// <returns>boolean value if we can not add the student to the couse</returns>
        public StudentSSN AddStudentToCourse(int id, StudentSSN student){
            
            Console.WriteLine("AddStudentToCourse");
            var isStudentInCourse  = (from x in _db.StudentsInCourses
            join ct in _db.Students on x.SSN equals ct.SSN 
            where x.CourseID == id
            where x.SSN == student.SSN
            select new StudentDTO{
                 ID = ct.ID,
                 SSN = ct.SSN,
                 Name = ct.Name
             }).SingleOrDefault();


            Console.WriteLine("AddStudentToCourse CHECK");
            if(isStudentInCourse == null)
             {
                 Console.WriteLine("AddStudentToCourse INSIDECHECK");
                 Console.WriteLine(isStudentInCourse.SSN);
                 throw new StudentIsInCourseException();
             }
            // var studentExists = (from x in _db.Students
            // where student.SSN == (long)x.SSN
            // select new StudentSSN{
            //       SSN = x.SSN
            // }).SingleOrDefault();

            /* if(studentExists == null)
             {
                 throw new StudentNonExistException();
              }   
                 */

                 Console.WriteLine("AddStudentToCourse STUDENTSINCOURSE");
            var studentInCourse = new Entities.StudentsInCourse {
                    CourseID = id,
                    SSN =student.SSN
            };

            _db.StudentsInCourses.Add(studentInCourse);

            try {
                _db.SaveChanges();
            } catch (Exception e) {
                Console.WriteLine(e);
                throw new FailedToSaveToDatabaseException();
            }

            Console.WriteLine("WE RETURN STUDENT");
            return student;
        }

         /// <summary>
         /// Deleats couse by Id
         /// </summary>
         /// <param name="id">The id of the couse we want to delete</param>
         /// <returns>Boolean value if we could not remove from database</returns>
         public CoursesDTO DeleteCourse(int id){
             Console.WriteLine("DeleteCourse");
            var corseToDelete = 
            (from x in _db.Courses
             where x.ID == id
             select x).SingleOrDefault();

           var courseToReturn = 
            (from x in _db.Courses
             where x.ID == id
             select new CoursesDTO{
                 ID = x.ID,
                 TemplateID = x.TemplateID,
                 Semester = x.Semester,
                 StartDate = x.StartDate,
                 EndDate = x.EndDate
             }).SingleOrDefault();

            if(corseToDelete == null)
            {
                throw new AppObjectNotFoundException();
            }
            else
            {
                 _db.Courses.Remove(corseToDelete);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new FailedToSaveToDatabaseException();
                }
            }
            return courseToReturn;
         }
     }
}
