using Api_ELearning.DependencyInjections;
using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.Repositories
{
    public interface IQuestionRepository: IRepository<Question>
    {
        int addQuestion(Question question);
        IEnumerable<Question> getListQuestion(int idExam);
    }
}
