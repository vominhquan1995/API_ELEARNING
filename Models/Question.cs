using System;
using System.Collections.Generic;
using System.Text;

namespace Api_ELearning.Models
{
    public class Question : Base
    {
        public string Content { get; set; }
        public int Level { get; set; }
        public int? ExamId { get; set; }
        public virtual Exam Exam { get; set; }
        public virtual ICollection<ResultQuestion> ResultQuestions { get; set; }
    }
}
