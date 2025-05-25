using EducationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher{ get; set; }

        public DbSet<Subject> Subject { get; set; }

        public DbSet<StudentSubject> StudentSubject { get; set; }

        public DbSet<TeacherSubject> TeacherSubject { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentSubject>()
        .HasKey(ss => ss.Id);

            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.StudentId);

            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Subject)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Teacher)
                .WithMany(t => t.StudentSubjects)
                .HasForeignKey(ss => ss.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Teacher)
                .WithMany(t => t.TeacherSubjects)
                .HasForeignKey(ts => ts.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany(s => s.TeacherSubjects)
                .HasForeignKey(ts => ts.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
