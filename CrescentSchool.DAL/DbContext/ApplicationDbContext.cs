using CrescentSchool.API.Entities;
using CrescentSchool.Models;
namespace CrescentSchool.DAL.DbContext;

using CrescentSchool.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }
    public DbSet<Student> Students { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<StudentMonthlyReport> StudentMonthlyReports { get; set; }
}
