using Api_ELearning.Data;
using Microsoft.Extensions.Logging;
using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_ELearning.DependencyInjections;
using Api_ELearning.ActionModels;
using Microsoft.EntityFrameworkCore;

namespace Api_ELearning.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly ELearningDbContext _context;
        private readonly ILogger _log;

        public ExamRepository(ELearningDbContext context, ILogger<ExamRepository> log)
        {
            _context = context;
            _log = log;
        }
        public IEnumerable<Exam> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.Exams.ToList();
            }
            set { }
        }

        public int AddExam(Exam o)
        {
            try
            {
                _log.LogInformation("BEGIN => Add");
                _context.Exams.Add(o);
                _context.SaveChanges();
                _log.LogInformation("END <= Add");
                return o.Id;
            }
            catch (Exception e)
            {
                _log.LogError("ERROR <= Add : [%s]", e);
                return -1;
            }
        }

        public bool Edit(Exam o)
        {
            try
            {
                _log.LogInformation("BEGIN => Edit");
                _context.Update(o);
                _context.SaveChanges();
                _log.LogInformation("END <= Edit");
                return true;
            }
            catch (Exception e)
            {
                _log.LogError("ERROR => Edit : [%s]", e);
                return false;
            }
        }

        public Exam Get(int idLesson)
        {
            var historyExam = _context.ExamDetailses.Include(a => a.Exam).Where(b => b.Exam.LessonId == idLesson);
            if (historyExam != null)
            {
                return _context.Exams.Where(b => b.LessonId == idLesson).First();
            }
            else
            {
                //get random one exam of lesson      
                return _context.Exams.Where(a => a.LessonId == idLesson).OrderBy(q => Guid.NewGuid()).First();
            }
        }

        public bool Remove(int id)
        {
            try
            {
                _log.LogInformation("BEGIN => Remove");
                _context.Exams.Remove(_context.Exams.Find(id));
                _context.SaveChanges();
                _log.LogInformation("END <= Remove");
                return true;
            }
            catch (Exception e)
            {
                _log.LogError("ERROR => Remove : [%s]", e);
                return false;
            }
        }
        public IEnumerable<Question> RandomQuestion(int idExam)
        {
            int number = _context.Exams.Where(a => a.Id == idExam).Select(a => a.NumberQuestion).FirstOrDefault();
            return (_context.Questions.Where(a => a.Level == 1 && a.ExamId == idExam).OrderBy(q => Guid.NewGuid()).Take((int)Math.Round(number / 2.0, MidpointRounding.AwayFromZero)))
                .Concat(_context.Questions.Where(a => a.Level == 2 && a.ExamId == idExam).OrderBy(q => Guid.NewGuid()).Take((int)Math.Round(number / 4.0, MidpointRounding.AwayFromZero)))
                .Concat(_context.Questions.Where(a => a.Level == 3 && a.ExamId == idExam).OrderBy(q => Guid.NewGuid()).Take((int)Math.Round(number / 4.0, MidpointRounding.AwayFromZero))).ToList() ;
        }
        public IEnumerable<QuestionActionModel> RandomQuestionNew(int idExam)
        {
            List<QuestionActionModel> listQuestionReturn = new List<QuestionActionModel>();
            List<Question> listQuestion = new List<Question>();
            int number = _context.Exams.Where(a => a.Id == idExam).Select(a => a.NumberQuestion).FirstOrDefault();
            listQuestion = (_context.Questions.Where(a => a.Level == 1 && a.ExamId == idExam).OrderBy(q => Guid.NewGuid()).Take((int)Math.Round(number / 2.0, MidpointRounding.AwayFromZero)))
                .Concat(_context.Questions.Where(a => a.Level == 2 && a.ExamId == idExam).OrderBy(q => Guid.NewGuid()).Take((int)Math.Round(number / 4.0, MidpointRounding.AwayFromZero)))
                .Concat(_context.Questions.Where(a => a.Level == 3 && a.ExamId == idExam).OrderBy(q => Guid.NewGuid()).Take((int)Math.Round(number / 4.0, MidpointRounding.AwayFromZero))).ToList();
            foreach(var item in listQuestion)
            {             
                List<AnswerModel> listAnswer = new List<AnswerModel>();
                var listReuslt=_context.ResultQuestions.Where(a => a.Question.Id == item.Id).ToList();
                foreach(var answer in listReuslt)
                {
                    listAnswer.Add(new AnswerModel
                    {
                        IdAnswer = answer.Id,
                        ContentAnswer = answer.Content
                    });
                }
                listQuestionReturn.Add(new QuestionActionModel
                {
                    IdQuestion=item.Id,
                    Content=item.Content,
                    listAnswer=listAnswer
                });
            }
            return listQuestionReturn;
        }
        public IEnumerable<Question> ExamCourses(int idCourses, int numberQuestion)
        {
            var listQuestion=new List<Question>();
            var listLesson = _context.Lessons.Where(a => a.CourseId == idCourses).ToList();
            foreach( var lesson in listLesson)
            {
                    int idExam = _context.Exams.Where(b => b.Lesson.Id == lesson.Id).Select(b => b.Id).FirstOrDefault();
                    listQuestion.AddRange(_context.Questions.Where(a => a.ExamId == idExam).ToList());             
            }   
            return listQuestion.OrderBy(a=>Guid.NewGuid()).Take(numberQuestion);
        }
        public string UpdateResultExam(int idUser,int idLesson, int numberRight)
        {
            string resultexam = null;
            try
            {
                int totalQuestion= _context.Exams.Where(a => a.Id == idLesson).Select(a => a.NumberQuestion).FirstOrDefault();
                //check if not exist is add new else if update point and status
                if (_context.ExamDetailses.Where(a => a.ExamId == idLesson && a.AccountId == idUser).FirstOrDefault() == null)
                {
                    _context.ExamDetailses.Add(new ExamDetails
                    {
                        AccountId = _context.Accounts.Where(a => a.Id == idUser).FirstOrDefault().Id,
                        ExamId = _context.Exams.Where(a => a.Id == idLesson).FirstOrDefault().Id,
                        Status = CheckPast(numberRight,totalQuestion) > 75.0 ? true : false,
                        Point = (int)Math.Round(CheckPast(numberRight, totalQuestion), MidpointRounding.AwayFromZero)
                    });
                }
                else
                {
                    ExamDetails lastExam = _context.ExamDetailses.Where(a => a.ExamId == idLesson && a.AccountId == idUser).FirstOrDefault();
                    lastExam.Status = CheckPast(numberRight, totalQuestion) > 75.0 ? true : false;
                    lastExam.Point = (int)Math.Round(CheckPast(numberRight, totalQuestion), MidpointRounding.AwayFromZero);
                    lastExam.EditTime = DateTime.Now;
                    _context.ExamDetailses.Update(lastExam);
                    _context.SaveChanges();
                }
                //update progress learn
                // find column value with idLesson - idUser - idCourses
                LessonDetails learn = _context.LessonDetailses
                    .Where(a => a.LessonId == _context.Exams.Where(b => b.Id == idLesson).FirstOrDefault().LessonId
                && a.AccountId == idUser).FirstOrDefault();
                if (learn== null)
                {
                    _context.LessonDetailses.Add(new LessonDetails
                    {
                        AccountId = idUser,
                        LessonId = idLesson,
                        Status = CheckPast(numberRight, totalQuestion) > 75.0 ? true : false,
                        EditTime=DateTime.Now
                    });
                    resultexam = CheckPast(numberRight, totalQuestion) > 75.0 ? "Chúc mừng. Bạn đã vượt qua" :"Rất tiếc. Bạn chưa đạt yêu cầu";
                }
                else
                {
                    learn.Status = CheckPast(numberRight, totalQuestion) > 75.0 ? true : false;
                    learn.EditTime = DateTime.Now;
                    resultexam = learn.Status == true ? "Chúc mừng. Bạn đã vượt qua" : "Rất tiếc. Bạn chưa đạt yêu cầu";
                    _context.LessonDetailses.Update(learn);
                }
                _context.SaveChanges();           
            }
            catch
            {
                resultexam = null;
            }
            return resultexam;

        }
        public string UpdateFinalExam(int idUser, int idCourses, int numberRight,int numberQuestion)
        {
            string resultFinalExam = null;
            try
            {
                //check if not exist is add new else if update point and status
                if (_context.CourseDetailses.Where(a => a.CourseId == idCourses && a.AccountId == idUser).FirstOrDefault() == null)
                {
                    _context.CourseDetailses.Add(new CourseDetails
                    {
                        AccountId = _context.Accounts.Where(a => a.Id == idUser).FirstOrDefault().Id,
                        CourseId = _context.Courses.Where(a => a.Id == idCourses).FirstOrDefault().Id,
                        Status = CheckPast(numberRight,numberQuestion) > 75.0 ? true : false
                    });
                }
                else
                {
                    CourseDetails lastfinalExam = _context.CourseDetailses.Where(a => a.CourseId == idCourses && a.AccountId == idUser).FirstOrDefault();
                    lastfinalExam.Status = CheckPast(numberRight, numberQuestion) > 75.0 ? true : false;
                    _context.CourseDetailses.Update(lastfinalExam);
                }
                _context.SaveChanges();
                resultFinalExam = CheckPast(numberRight,numberQuestion) > 75.0 ? "Chúc mừng bạn đã hoàn thành khóa học !!!" : "Rất tiếc. Bạn chưa vượt qua bài kiểm tra cuối khóa";
            }
            catch
            {
                resultFinalExam = null;
            }
            return resultFinalExam;
        }
        public double CheckPast(int numberRight,int total)
        {
            return (((double)numberRight / total)*100);
        }
        public int CheckAnswer(int idExam,string[] listID){
            int countRight = 0;
            for(int i=0;i < listID.Length; i++)
            {
                int id = Int32.Parse(listID[i].ToString());
                if (_context.ResultQuestions.Where(a => a.Id ==id)
                .Select(a => a.Status).First())
                {
                    countRight++;
                }        
             }
            return countRight;
        }
        bool IRepository<Exam>.Add(Exam o)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Exam> getListExam(int idLesson)
        {
            return _context.Exams.Where(a => a.LessonId == idLesson).ToList();
        }
        public IEnumerable<ExamDetailsActionModel> listHistoryExam(int idUser)
        {
            return _context.ExamDetailses.Include(k => k.Exam).Where(a => a.AccountId == idUser).Select(b => new ExamDetailsActionModel
                    {
                        ExamName = b.Exam.ExamName,
                        LessonId=b.Exam.LessonId,
                        ExamId = b.ExamId,
                        Time = b.EditTime,
                        Point = b.Point,
                        Status = b.Status == true ? "Hoàn Thành" : "Chưa Hoàn Thành"
                    })
              .ToList();
        }
    }
}
