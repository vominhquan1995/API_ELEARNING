using Api_ELearning.ActionModels;
using Api_ELearning.DependencyInjections;
using Api_ELearning.Models;
using System.Collections.Generic;

namespace Api_ELearning.Repositories
{
    public interface IExamRepository : IRepository<Exam>
    {
        IEnumerable<Question> RandomQuestion(int idExam);
        IEnumerable<QuestionActionModel> RandomQuestionNew(int idExam);
        IEnumerable<Question> ExamCourses(int idCourses,int numberQuestion);
        string UpdateResultExam(int idUser, int idExam, int numberRight);
        string UpdateFinalExam(int idUser, int idCourses, int numberRight, int numberQuestion);
        int CheckAnswer(int idExam, string[] listID);
        IEnumerable<Exam> getListExam(int idLesson);
        int AddExam(Exam o);
        IEnumerable<ExamDetailsActionModel> listHistoryExam(int idUser);
    }
}
