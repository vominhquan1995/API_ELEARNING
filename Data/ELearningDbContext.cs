using Microsoft.EntityFrameworkCore;
using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Api_ELearning.Data
{
    public class ELearningDbContext : DbContext
    {
        public ELearningDbContext(DbContextOptions<ELearningDbContext> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        //add more table - dbset
        public DbSet<Role> Roles { get; set; }

        //add table cetificate
        public DbSet<Cetificate> Cetificates { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<ResultQuestion> ResultQuestions { get; set; }

        public DbSet<Rate> Rates { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<LessonDetails> LessonDetailses { get; set; }

        public DbSet<ExamDetails> ExamDetailses { get; set; }

        public DbSet<CourseDetails> CourseDetailses { get; set; }

        public DbSet<Category> Categorys { get; set; }

        public DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>()
                .HasKey(o => o.Id);
            builder.Entity<Account>().HasKey(o => o.Id);
            builder.Entity<Account>()
                .HasOne(o=>o.Role)
                .WithMany()
                .HasForeignKey(o=>o.RoleId);
            builder.Entity<Account>()
                .HasOne(o => o.Role)
                .WithMany(k => k.Accounts)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<ExamDetails>()
                .HasKey(k => new { k.AccountId, k.ExamId});    
            builder.Entity<ExamDetails>()
                .HasOne(o => o.Account)
                .WithMany(m => m.ExamDetailses)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ExamDetails>()
                .HasOne(o => o.Exam)
                .WithMany(m => m.ExamDetailses)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Exam>()
                .HasKey(o => o.Id);
            builder.Entity<Exam>()
                .HasOne(o => o.Lesson)
                .WithMany()
                .HasForeignKey(f => f.LessonId);
            builder.Entity<Exam>()
                .HasOne(o => o.Lesson)
                .WithMany(m => m.Exams)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Question>()
                .HasKey(o => o.Id);
            builder.Entity<Question>()
                .HasOne(o => o.Exam)
                .WithMany()
                .HasForeignKey(f => f.ExamId);
            builder.Entity<Question>()
                .HasOne(o => o.Exam)
                .WithMany(m => m.Questions)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<ResultQuestion>()
                .HasKey(o => o.Id);
            builder.Entity<ResultQuestion>()
                .HasOne(o => o.Question)
                .WithMany()
                .HasForeignKey(f => f.QuestionId);
            builder.Entity<ResultQuestion>()
                .HasOne(o => o.Question)
                .WithMany(m => m.ResultQuestions)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<LessonDetails>()
                .HasKey(k => new { k.AccountId, k.LessonId});
            builder.Entity<LessonDetails>()
                .HasOne(o => o.Account)
                .WithMany(m => m.LessonDetailses)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<LessonDetails>()
                .HasOne(o => o.Lesson)
                .WithMany(m => m.LessonDetailses)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CourseDetails>()
                .HasKey(k => new { k.AccountId, k.CourseId});
            builder.Entity<CourseDetails>()
                .HasOne(o => o.Account)
                .WithMany(m => m.CourseDetailses)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CourseDetails>()
                .HasOne(o => o.Course)
                .WithMany(m => m.CourseDetailses)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Rate>()
                .HasOne(o => o.Account)
                .WithMany(m => m.Rates)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Rate>()
                .HasOne(o => o.Course)
                .WithMany(m => m.Rates)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Course>()
                .HasKey(k => k.Id);
            builder.Entity<Course>()
                .HasOne(o => o.Account)
                .WithMany(m => m.Courseses)
                .HasForeignKey(f => f.AccountId);
            builder.Entity<Course>()
               .HasOne(o => o.Account)
               .WithMany(m => m.Courseses)
               .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Course>()
                .HasOne(o => o.Category)
                .WithMany(m => m.Courseses)
                .HasForeignKey(f => f.CategoryId);
            builder.Entity<Course>()
               .HasOne(o => o.Category)
               .WithMany(m => m.Courseses)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Course>()
                .HasOne(o => o.Cetificate)
                .WithMany(m => m.Courseses)
                .HasForeignKey(f => f.CetificateId);
            builder.Entity<Course>()
               .HasOne(o => o.Cetificate)
               .WithMany(m => m.Courseses)
               .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Lesson>()
                .HasKey(o => o.Id);
            builder.Entity<Lesson>()
                .HasOne(o => o.Course)
                .WithMany(m => m.Lessons)
                .HasForeignKey(f => f.CourseId);
            builder.Entity<Lesson>()
               .HasOne(o => o.Course)
               .WithMany(m => m.Lessons)
               .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Category>()
                .HasKey(o => o.Id);
            builder.Entity<Cetificate>()
                .HasKey(o => o.Id);
            builder.Entity<Notification>()
               .HasKey(o => o.Id);

        }
    }
}
