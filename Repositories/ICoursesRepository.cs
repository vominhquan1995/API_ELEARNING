using Api_ELearning.ActionModels;
using Api_ELearning.DependencyInjections;
using Api_ELearning.Models;
using System.Collections.Generic;

namespace Api_ELearning.Repositories
{
    public interface ICoursesRepository: IRepository<Course>
    {
        IEnumerable<CoursesActionModel> ListCourse();
        IEnumerable<CoursesActionModel> ListCoursesTeacher(int idTeacher);
        IEnumerable<CoursesActionModel> ListCoursesCategory(int idCategory);
        IEnumerable<CoursesActionModel> ListMostPopular(int numberCourses);
        IEnumerable<CoursesActionModel> ListNewCourses(int numberCourses);
        IEnumerable<CoursesActionModel> ListTopReview(int numberCourses);
        IEnumerable<CoursesActionModel> Search(string search);
        CoursesActionModel GetDetailCourses(int idCourses);
        IEnumerable<CoursesActionModel> FilterPrice(decimal from, decimal to);
        IEnumerable<CoursesActionModel> FilterRate(decimal from, decimal to);
        IEnumerable<CoursesActionModel> FilterCategory(int idCategory);
        IEnumerable<CoursesActionModel> getListCoursePage(int numberItem,int  indexPage);


    }
}
