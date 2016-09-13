using System;
using System.Collections.Generic;
using Assignment3.API.Models;
using Assignment3.API.Services.Entities;
using System.Linq;

namespace Assignment3.API.Services
{
    public class CoursesService : ICoursesService
    {
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
        /// <returns>List of courses<CourseDTOLite></returns>
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
        /// <returns>Returns CourseDetailed  if the course excists in the database 
        /// else it returns AppObjectNotFoundException()
        ///</returns>
       public CourseDetailed GetCourseByID(int id){  

            var listOfStudents2 = (from x in _db.StudentsInCourses
            join ct in _db.Students on x.SSN equals ct.SSN 
            where x.CourseID == id
            where x.Active == 1
            select new StudentDTO{
                 Name = ct.Name,
                 SSN = ct.SSN,
             }).ToList(); 
             var course = (from x in _db.Courses
             join ct in _db.CourseTemplate on x.TemplateID equals ct.TemplateID
             where x.ID == id
              select new CourseDetailed{
                  ID = x.ID,
                  Name = ct.Name,
                  TemplateID = x.TemplateID,
                  Semester = x.Semester,
                  StartDate  = x.StartDate,
                  EndDate = x.EndDate,
                  listOfStudents = listOfStudents2,
                  MaxStudents = x.MaxStudents
              }).SingleOrDefault();

              if(course == null)
              {
                  throw new AppObjectNotFoundException();
              }
              return course;
         }

        /// <summary>
        /// Function that returns a list of all students in a spesific course
        /// </summary>
        /// <param name="id">The id of the couse</param>
        /// <returns>List of students<StudentDTO></returns>
        public List<StudentDTO> GetListOfStudentsByCourseId(int id){
            //Only list active users not that have been deleted(SaveChanges)

            var course = GetCourseByID(id);
            var listOfStudents = (from x in _db.StudentsInCourses
            join ct in _db.Students on x.SSN equals ct.SSN 
            where x.CourseID == id
            where x.Active == 1
            select new StudentDTO{
                 Name = ct.Name,
                 SSN = ct.SSN,
             }).ToList();
             return listOfStudents;
         }

        /// <summary>
        /// Function that returns a waiting list for students that want to join a course
        /// </summary>
        /// <param name="id">The id of the course </param>
        /// <returns>List of students that are on the waitinglist</returns>
        public List<StudentDTO> GetWaitingList(int id)
        {
            var course = GetCourseByID(id);

            var listOfStudentsInWaitingList = (from x in _db.WaitingList
            join stud in _db.Students on x.SSN equals stud.SSN
            select new StudentDTO{  
                Name = stud.Name,
                SSN = stud.SSN,
            }).ToList();

            return listOfStudentsInWaitingList;
        }

         /// <summary>
         /// Allows us to update the startdate and endate and the maxStudents value of the course. 
         /// </summary>
         /// <param name="id">The id of the course we want to update</param>
         /// <param name="coursedt">instance of CourseUpdate that we can get startdate, enddate and MaxStudents</param>
         /// <returns>The course we created or FailedToSaveToDatabaseException() if we can't save to the database</returns>
         public CoursesDTO UpdateCourse(int id,CourseUpdate coursedt)
         {
             var courseToUpdate = (from x in _db.Courses
             where x.ID == id
             select x).SingleOrDefault();
             
             if(courseToUpdate == null)
             {
                 throw new AppObjectNotFoundException();
             }
             else
             {  
                 courseToUpdate.StartDate = coursedt.StartDate;
                 courseToUpdate.EndDate   = coursedt.EndDate;
                 courseToUpdate.MaxStudents = coursedt.MaxStudents;
                 
                 try
                 {
                     _db.SaveChanges();
                 }
                 catch (System.Exception)
                 {
                    throw new FailedToSaveToDatabaseException();
                 }
             }

            var courseToReturn = (from x in _db.Courses
            where x.ID == id
            select  new CoursesDTO{
                 ID = x.ID,
                 TemplateID = x.TemplateID,
                 Semester = x.Semester,
                 StartDate = x.StartDate,
                 EndDate = x.EndDate,
                 MaxStudents = x.MaxStudents
             }).SingleOrDefault();

             return courseToReturn;
         }
 
       /// <summary>
       /// Creates a new course
       /// </summary>
       /// <param name="course">Instance of AddCourse that keeps the courses value</param>
       /// <returns>Returns the new course if we can save changes to database, else we throw
       /// FailedToSaveToDatabaseException()
       public Courses CreateCourse(AddCourse course) 
       {
           var newCourse = new Entities.Courses {
               TemplateID = course.TemplateID,
               StartDate = course.StartDate,
               EndDate = course.EndDate,
               Semester = course.Semester,
               MaxStudents = course.MaxStudents
           };
            _db.Courses.Add(newCourse);
            try
            {
                _db.SaveChanges();
            }
            catch (System.Exception)
            {
                throw new FailedToSaveToDatabaseException();
            }
            
           return newCourse;
       }

        /// <summary>
        /// Adds student to the table StudentsInCourse 
        /// </summary>
        /// <param name="id">The id of the couse we are going to add in</param>
        /// <param name="student">The Student of type StudentSSN containing the students SSN </param>
        /// <returns>If maximum number of students has reached we throw MaxNumberOfStudentsException().
        /// If the student does not exsist in the database we throw StudentNonExistException().
        /// If it fails to save to database we return FailedToSaveToDatabaseException().
        /// If nothing fails we return the student that was created. 
        /// ///</returns>
        public StudentSSN AddStudentToCourse(int id, StudentSSN student){
            
            Console.WriteLine("AddStudentToCourse");
           
           List<StudentDTO> listofStudents = GetListOfStudentsByCourseId(id);
           var numberOfStudents = listofStudents.Count;
           var course = GetCourseByID(id);

           if(numberOfStudents == course.MaxStudents )
           {
               throw new MaxNumberOfStudentsException();
           }
           
            var studentExists = (from x in _db.Students
            where student.SSN == x.SSN
            select new StudentSSN{
                   SSN = x.SSN
            }).SingleOrDefault();

            if(studentExists == null)
            {
                throw new StudentNonExistException();
            }   

            var isStudentInCourse = (from x in _db.StudentsInCourses
              where x.SSN == student.SSN
              where x.CourseID == id
              select x).SingleOrDefault();

            if(isStudentInCourse == null)
            {
                var studentInCourse = new Entities.StudentsInCourse {
                    CourseID = id,
                    SSN = student.SSN,
                    Active = 1
                };
                _db.StudentsInCourses.Add(studentInCourse);
            }
            else
            {
                if(isStudentInCourse.Active == 1)
                {
                    throw new StudentIsInCourseException();
                }
                else{
                    isStudentInCourse.Active = 1;
                }
            }

            var studentonwaitinglist = (from x in _db.WaitingList
            where x.SSN == student.SSN
            where x.CourseID == id
            select x).SingleOrDefault();

            if(studentonwaitinglist != null)
            {
               _db.WaitingList.Remove(studentonwaitinglist);
            }

            try {
                _db.SaveChanges();
            } catch (Exception e) {
                Console.WriteLine(e);
                throw new FailedToSaveToDatabaseException();
            }

            return student;
        }

         /// <summary>
         /// Should add a student to the waiting list of a spesific course
         /// </summary>
         /// <param name="id">The id of the Course</param>
         /// <param name="student">The student we are adding to the waiting list</param>
         /// <returns> If the student does not exist int the database we throw StudentNonExistException().
         /// If the student is alredy on the waiting list we throw StudentOnWaitingListException().
         /// If the student is alredy in the course that the waiting list is for we throw StudentIsInCourseException(). 
         /// If it fails to save to database we throw FailedToSaveToDatabaseException().
         /// Else we return the student that we added to the waiting list.
         /// </returns>
         public StudentSSN AddToWaitingList(int id,StudentSSN student)
        {
             var course = GetCourseByID(id);

             var newStudent = new Entities.WaitingList{
                CourseID = id,
                SSN = student.SSN
                };            
             var studentExists = (from x in _db.Students
             where student.SSN == x.SSN
             select new StudentSSN{
                   SSN = x.SSN
             }).SingleOrDefault();

             if(studentExists == null)
             {
                 throw new StudentNonExistException();
            }   
            List<StudentDTO> students = GetWaitingList(id);
            var studentinlist = students.Exists(x => x.SSN == student.SSN);

            if(studentinlist)
            {
                throw new StudentOnWaitingListException();
            }
            List<StudentDTO> studentsInCourse = GetListOfStudentsByCourseId(id);
            var studentInCourse = studentsInCourse.Exists(x => x.SSN == student.SSN);

            if(studentInCourse)
            {
                throw new StudentIsInCourseException();
            }
           _db.WaitingList.Add(newStudent);
             try {
                _db.SaveChanges();
            } catch (Exception e) {
                Console.WriteLine(e);
                throw new FailedToSaveToDatabaseException();
            }
            return student;
        }
        
          /// <summary>
          /// Deleats a student from a course. 
          /// </summary>
          /// <param name="id">Id of the course</param>
          /// <param name="SSN">The social security number of the student</param>
          /// <returns>The student deleted SSN if we are able to save to database else FailedToSaveToDatabaseException().</returns>
          public StudentSSN DeleteStudent(int id, long SSN){

              var course = GetCourseByID(id);
              StudentSSN studentdelete = new StudentSSN();
              studentdelete.SSN = SSN;

              var listStudents = course.listOfStudents;
              var studentinCourse = listStudents.Exists(x => x.SSN == SSN);
              if(studentinCourse == false)
              {
                  throw new AppObjectNotFoundException();
              }
              //else we find and delete
              listStudents.RemoveAll(x => x.SSN == SSN);

              var student2 = (from x in _db.StudentsInCourses
              where x.SSN == SSN
              select x).SingleOrDefault();

              student2.Active = 0;

              try {
                _db.SaveChanges();
              } 
              catch (Exception e) {
                Console.WriteLine(e);
                throw new FailedToSaveToDatabaseException();
            }
              return studentdelete;
          }

         /// <summary>
         /// Deleats a course
         /// </summary>
         /// <param name="id">The id of the course to delete </param>
         /// <returns>The deleted course if the course is found else it returns AppObjectNotFoundException()</returns>
         /// or an FailedToSaveToDatabaseException() if we could not save to the database
         public CoursesDTO DeleteCourse(int id){
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
