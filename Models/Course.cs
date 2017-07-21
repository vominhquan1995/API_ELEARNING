using System;
using System.Collections.Generic;
using System.Text;

namespace Api_ELearning.Models
{
    public class Course : Base
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int NumberCredits { get; set; }
        public string Donors { get; set; }
        public string UrlImage { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int? AccountId { get; set; }
        public virtual Account Account { get; set; } //teacher
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int? CetificateId { get; set; }
        public virtual Cetificate Cetificate { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<CourseDetails> CourseDetailses { get; set; }
    }
}
