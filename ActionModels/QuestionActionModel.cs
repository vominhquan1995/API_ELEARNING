using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.ActionModels
{
    public class QuestionActionModel
    {
        public int IdQuestion { get; set; }
        public string Content { get; set; }
        public List<AnswerModel> listAnswer{ get; set; }
    }
    public class AnswerModel
    {
        public int IdAnswer { get; set; }
        public string ContentAnswer { get; set; }
    }
}
