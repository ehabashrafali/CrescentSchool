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
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<StudentMonthlyReport> StudentMonthlyReports { get; set; }
    public DbSet<UpComingSessions> UpCommingSessions { get; set; }
    public DbSet<ZoomWebhookInboxMessage> ZoomWebhookInboxMessages { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().HasQueryFilter(u => !u.IsDeleted);
        builder.Entity<Student>().HasQueryFilter(u => !u.User.IsDeleted);
        builder.Entity<Instructor>().HasQueryFilter(u => !u.User.IsDeleted);
        builder.ApplyConfiguration(new Configurations.ParentConfiguration());
        builder.Entity<Parent>().HasQueryFilter(p => !p.User.IsDeleted);
        builder.Entity<Session>().HasQueryFilter(s => !s.IsDeleted);
    }
}
