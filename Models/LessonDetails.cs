using System;
using System.Collections.Generic;
using System.Text;

namespace Api_ELearning.Models
{
    public class LessonDetails
    {
        public bool Status { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; }
        public DateTime EditTime { get; set; } = DateTime.Now;
    }
}
