using Api_ELearning.Data;
using Microsoft.Extensions.Logging;
using Api_ELearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_ELearning.ActionModels;
using Microsoft.EntityFrameworkCore;

namespace Api_ELearning.Repositories
{
    public class CourseDetailsRepository :ICourseDetailsRepository
    {
        private readonly ELearningDbContext _context;
        private readonly ILogger _log;

        public CourseDetailsRepository(ELearningDbContext context, ILogger<CourseDetailsRepository> log)
        {
            _context = context;
            _log = log;
        }
        public IEnumerable<CourseDetails> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.CourseDetailses.ToList();
            }
            set { }
        }

        public bool Add(CourseDetails o)
        {
            try
            {
                _log.LogInformation("BEGIN => Add");
                _context.CourseDetailses.Add(o);
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

        public bool Edit(CourseDetails o)
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

        public CourseDetails Get(int id)
        {
            _log.LogInformation("GET => [%s]", id);
            return _context.CourseDetailses.Find(id);
        }

        public bool Remove(int id)
        {
            try
            {
                _log.LogInformation("BEGIN => Remove");
                _context.CourseDetailses.Remove(_context.CourseDetailses.Find(id));
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
        public IEnumerable<HistoryLearnActionModel> GetCourseDetails(int idUser)
        {
            return _context.CourseDetailses.Include(o=>o.Course).Where(a => a.AccountId == idUser).Select(a => new HistoryLearnActionModel
            {
                        id = a.CourseId,
                        nameCourses = a.Course.Name,
                        urlImage=a.Course.UrlImage,
                        status = a.Status
                    }).ToList();
        }
    }
}
