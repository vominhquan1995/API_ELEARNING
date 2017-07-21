using System;
using System.Collections.Generic;
using System.Text;

namespace Api_ELearning.Models
{
    public class Category:Base
    {
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public virtual ICollection<Course> Courseses { get; set; }
    }
}
