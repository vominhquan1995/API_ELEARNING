using System;
using System.Collections.Generic;
using System.Text;

namespace Api_ELearning.Models
{
    public class Exam : Base
    {
        public string ExamName { get; set; }
        public int NumberQuestion { get; set; }
        public int TimeExam { get; set; }
        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<ExamDetails> ExamDetailses { get; set; }
    }
}
