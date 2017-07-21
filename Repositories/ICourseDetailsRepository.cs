using Api_ELearning.ActionModels;
using Api_ELearning.DependencyInjections;
using Api_ELearning.Models;
using System.Collections.Generic;

namespace Api_ELearning.Repositories
{
    public interface ICourseDetailsRepository : IRepository<CourseDetails>
    {
        IEnumerable<HistoryLearnActionModel> GetCourseDetails(int idUser);
    }
}
