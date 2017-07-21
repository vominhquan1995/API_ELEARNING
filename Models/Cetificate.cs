using System;
using System.Collections.Generic;
using System.Text;

namespace Api_ELearning.Models
{
     public class Cetificate:Base
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LeadTime { get; set; }
        public virtual ICollection<Course> Courseses { get; set; }
    }
}
