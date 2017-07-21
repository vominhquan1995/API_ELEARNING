using Api_ELearning.DependencyInjections;
using Api_ELearning.Models;
using System.Collections.Generic;

namespace Api_ELearning.Repositories
{
    public interface IResultQuestionRepository : IRepository<ResultQuestion>
    {
        IEnumerable<ResultQuestion> ListResult(int id);
        bool CheckAnswer(int id);
    }
}
