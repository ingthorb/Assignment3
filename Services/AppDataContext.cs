using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Assignment3.API.Services.Entities;

namespace Assignment3.API.Services
{
    public class AppDataContext : DbContext
    {

        public DbSet<Course> Courses {get;set;}
        public DbSet<CourseTemplate> CourseTemplate { get; set; }
        public DbSet<StudentsInCourse> StudentsInCourses { get; set; }
        public DbSet<Students> Students {get; set;}

        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
