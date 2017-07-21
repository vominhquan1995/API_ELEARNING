using Api_ELearning.ActionModels;
using Api_ELearning.DependencyInjections;
using Api_ELearning.Models;
using System.Collections.Generic;

namespace Api_ELearning.Repositories
{
    public interface IRateRepository : IRepository<Rate>
    {
        int GetRateTeacher(int idUSer);
        int GetRateCourses(int idCourses);
        IEnumerable<ReviewModel> GetReviewCourses(int idCourses);
        IEnumerable<ReviewModel> GetReviewTeacher(int idCourses);
    }
}
