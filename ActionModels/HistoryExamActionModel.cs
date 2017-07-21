using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.ActionModels
{
    public class ExamDetailsActionModel
    {
        public int ExamId { get; set; }
        public int LessonId { get; set; }
        public string ExamName { get; set;}
        public DateTime Time { get; set; }
        public int Point { get; set; }
        public string Status { get; set; }
    }
}
