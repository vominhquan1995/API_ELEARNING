using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.ActionModels
{
    public class CoursesActionModel:Base
    {
        public string NameCourses { get; set; }
        public string Donors { get; set; }
        public int NumberCredits { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Teacher { get; set; }
        public string Category { get; set; }
        public string Cetificate { get; set; }  
        public string UrlImage { get; set; }
        public int idTeacher { get; set; }
        public int idCategory { get; set; }
        public int idCetificate { get; set; }
    }
}
