using System;
using System.Collections.Generic;
using System.Text;

namespace Api_ELearning.Models
{
    public class Rate : Base
    {
        public string Content { get; set; }
        public int Star { get; set; }
        public int RateType { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }//user complain
    }
}
