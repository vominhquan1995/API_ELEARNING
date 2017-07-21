using Api_ELearning.ActionModels;
using Api_ELearning.DependencyInjections;
using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.Repositories
{
    public interface IExamDetailsRepository : IRepository<ExamDetails>
    {
        ExamDetails Get(int idUser, int idExam);
    }
}
