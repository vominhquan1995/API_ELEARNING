using System.Collections.Generic;

namespace Api_ELearning.Models
{
    public class Lesson : Base
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public int? CourseId { get; set; }
        public string LessonUrl { get; set; }
        public string SourceType { get; set; }
        public int Time { get; set; }
        public string UrlImage { get; set; }
        public string Description { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<LessonDetails> LessonDetailses { get; set; }
    }
}
