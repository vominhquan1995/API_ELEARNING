using System;
using System.Collections.Generic;
using System.Text;

namespace Api_ELearning.Models
{
    public class ResultQuestion : Base
    {
        public string Content { get; set; }
        public bool Status { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
