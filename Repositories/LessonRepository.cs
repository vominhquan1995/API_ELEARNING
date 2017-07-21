using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_ELearning.Models;
using Api_ELearning.Data;
using Microsoft.Extensions.Logging;

namespace Api_ELearning.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly ELearningDbContext _context;

        private readonly ILogger _log;

        public LessonRepository(ELearningDbContext context, ILogger<LessonRepository> log)
        {
            _context = context;
            _log = log;
        }
        public IEnumerable<Lesson> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.Lessons.ToList();
            }
            set { }
        }
        public bool Add(Lesson o)
        {
             try
            {
                _log.LogInformation("BEGIN => Add");
                _context.Lessons.Add(o);
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

        public bool Edit(Lesson o)
        {
           try
            {
                _log.LogInformation("BEGIN => Edit");
                _context.Lessons.Update(o);
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

        public Lesson Get(int id)
        {
             _log.LogInformation("GET => [%s]", id);
            return _context.Lessons.Find(id);
        }

        public bool Remove(int id)
        {
             try
            {
                _log.LogInformation("BEGIN => Remove");
                _context.Lessons.Remove(_context.Lessons.Find(id));
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
        public IEnumerable<Lesson> getListLesson(int idCourse)
        {
            return this._context.Lessons.Where(a => a.Course.Id == idCourse).ToList();
        }
    }
}
