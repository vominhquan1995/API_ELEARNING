using System;
using System.Collections.Generic;
using System.Text;

namespace Api_ELearning.Models
{
    public class ExamDetails
    {
        public int Point { get; set; }
        public bool Status { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public int ExamId { get; set; }
        public virtual Exam Exam { get; set; }
        public DateTime EditTime { get; set;} = DateTime.Now;
    }
}
