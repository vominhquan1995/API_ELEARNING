using System;
using System.Collections.Generic;
using System.Text;

namespace Api_ELearning.Models
{
    public class CourseDetails
    {
        public bool Status { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public DateTime EditTime { get; set;} = DateTime.Now; 
    }
}
