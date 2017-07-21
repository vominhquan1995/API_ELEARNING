using Api_ELearning.ActionModels;
using Api_ELearning.DependencyInjections;
using Api_ELearning.Models;
using System.Collections.Generic;

namespace Api_ELearning.Repositories
{
    public interface ILearnRepository : IRepository<LessonDetails>
    {
        int GetProgress(int idUser,int idCourse);
        IEnumerable<ProgressPlan> GetProgressPlan(int idUser, int idCourse);
        IEnumerable<ProgressPlan> GetLearningPlan(int idStudent);
        bool BuyCourse(int idUser, int idCourse);
    }
}
