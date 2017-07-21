using System;
using System.Collections.Generic;
using System.Text;

namespace Api_ELearning.Models
{
    public class Account : Base
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string UrlAvatar { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Infomation { get; set; }
        public string Description { get; set; }
        public int? RoleId { get; set; }
        public virtual Role Role { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Course> Courseses { get; set; }
        public virtual ICollection<LessonDetails> LessonDetailses { get; set; }
        public virtual ICollection<CourseDetails> CourseDetailses { get; set; }
        public virtual ICollection<ExamDetails> ExamDetailses { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
    }
}
