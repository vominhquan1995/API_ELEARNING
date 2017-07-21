using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.ActionModels
{
    public class ReviewModel
    {
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}
