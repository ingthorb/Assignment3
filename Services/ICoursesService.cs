using System;
using System.Collections.Generic;
using Assignment3.API.Models;
using Assignment3.API.Services.Entities;

namespace Assignment3.API.Services
{
    public interface ICoursesService
    {
        List<CourseDTOLite> GetCoursesBySemester(string semester );

        List<StudentDTO> GetListOfStudentsByCourseId(int id);

        CourseDetailed GetCourseByID(int id);

        CoursesDTO DeleteCourse(int id);

        StudentSSN AddStudentToCourse(int id, StudentSSN student);

        CoursesDTO UpdateCourse(int id, CourseUpdate coursedt);
        
        Courses CreateCourse(AddCourse courses);
    }
}
