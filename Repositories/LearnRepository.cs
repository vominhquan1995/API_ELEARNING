using Api_ELearning.ActionModels;
using Api_ELearning.Data;
using Microsoft.Extensions.Logging;
using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Api_ELearning.Repositories
{
    public class LearnRepository : ILearnRepository
    {
        private readonly ELearningDbContext _context;
        private readonly ILogger _log;

        public LearnRepository(ELearningDbContext context, ILogger<LearnRepository> log)
        {
            _context = context;
            _log = log;
        }
        public IEnumerable<LessonDetails> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.LessonDetailses.ToList();
            }
            set { }
        }

        public bool Add(LessonDetails o)
        {
            try
            {
                _log.LogInformation("BEGIN => Add");
                _context.LessonDetailses.Add(o);
                _context.SaveChanges();
                _log.LogInformation("END <= Add");
                return true;
            }
            catch (Exception e)
            {
                _log.LogError("ERROR <= Add : [%s]", e);
                return false;
            }
        }

        public bool Edit(LessonDetails o)
        {
            try
            {
                _log.LogInformation("BEGIN => Edit");
                _context.LessonDetailses.Update(o);
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

        public LessonDetails Get(int id)
        {
            _log.LogInformation("GET => [%s]", id);
            return _context.LessonDetailses.FirstOrDefault(o=>o.LessonId==id);
        }

        public bool Remove(int id)
        {
            try
            {
                _log.LogInformation("BEGIN => Remove");
                _context.LessonDetailses.Remove(_context.LessonDetailses.Find(id));
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
        //get precent done course of user
        public int GetProgress(int idUser, int idCourse)
        {
            double total = _context.LessonDetailses.Join(_context.Lessons, a=>a.LessonId,b=>b.Id, (a, b) => new { a,b })
                .Where(c=>c.b.CourseId== idCourse && c.a.AccountId==idUser).Count();
            double complete = _context.LessonDetailses.Join(_context.Lessons, a => a.LessonId, b => b.Id, (a, b) => new { a, b })
                .Where(c => c.b.CourseId == idCourse && c.a.Status== true && c.a.AccountId == idUser).Count();
            return complete == 0.0 ? 0 : (int)((complete / total) * 100);
        }
        //get tree progress learn of courses
        public IEnumerable<ProgressPlan> GetProgressPlan(int idUser, int idCourse)
        {
            var lessonOfCourse = _context.Courses.Include(o => o.Lessons)
                .FirstOrDefault(o => o.AccountId.Equals(idUser) && o.Id.Equals(idCourse)).Lessons.ToList();
            var listLessonDetails = new List<LessonDetails>();
            foreach(var item in lessonOfCourse)
            {
                listLessonDetails.AddRange(_context.LessonDetailses.Where(o => o.LessonId.Equals(item.Id) && o.AccountId.Equals(idUser)));
            }
            foreach(var item in listLessonDetails)
            {
                yield return new ProgressPlan
                {
                    IdCourses = idCourse,
                    IdLesson = item.LessonId,
                    NameLesson = lessonOfCourse.FirstOrDefault(o => o.Id.Equals(item.LessonId)).Name,
                    Status = item.Status
                };
            }
        }
        public IEnumerable<ProgressPlan> GetLearningPlan(int idStudent)
        {
            return _context.CourseDetailses.Where(a => a.AccountId == idStudent).Select(a=> new ProgressPlan {
                IdCourses = a.CourseId,
                Status = a.Status
            });
        }
        public bool BuyCourse(int idUser, int idCourse)
        {
            try
            {
                //add history learn course
                _context.CourseDetailses.Add(new CourseDetails
                {
                    AccountId = idUser,
                    CourseId = idCourse,
                    EditTime=DateTime.Now,
                    Status = false
                });
                //get list Lesson of Course
                var listLesson = _context.Lessons.Where(a => a.CourseId == idCourse).ToList();
                //add lessondetail
                foreach(var lesson in listLesson)
                {
                    _context.LessonDetailses.Add(new LessonDetails
                    {
                        AccountId = idUser,
                        LessonId = lesson.Id,
                        EditTime = DateTime.Now,                   
                        Status = false
                    });
                }
                _context.SaveChanges();
                return true;

            }catch
            {
                return false;
            }
           
        }
    }
}
