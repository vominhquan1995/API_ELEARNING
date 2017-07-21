using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.ActionModels
{
    public class ProgressPlan
    {
        public int IdCourses { get; set; }
        public int IdLesson { get; set; }
        public string NameLesson { get; set; }
        public bool Status { get; set; }
    }
}
