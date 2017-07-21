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
    public class RateCoursesRepository:IRateRepository
    {
        private readonly ELearningDbContext _context;
        private readonly ILogger _log;

        public RateCoursesRepository(ELearningDbContext context, ILogger<RateCoursesRepository> log)
        {
            _context = context;
            _log = log;
        }
        public IEnumerable<Rate> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.Rates.ToList();
            }
            set { }
        }

        public bool Add(Rate o)
        {
            try
            {
                _log.LogInformation("BEGIN => Add");
                _context.Rates.Add(o);
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

        public bool Edit(Rate o)
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

        public Rate Get(int id)
        {
            _log.LogInformation("GET => [%s]", id);
            return _context.Rates.Find(id);
        }

        public bool Remove(int id)
        {
            try
            {
                _log.LogInformation("BEGIN => Remove");
                _context.Rates.Remove(_context.Rates.Find(id));
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
        public int GetRateTeacher(int idTeacher)
        {
            double star = 0;
            int count = 0;
            IEnumerable<Course> listCourses= _context.Courses.Where(a => a.AccountId == idTeacher ).ToList();
            foreach(var _courses in listCourses )
            {
                //check exist value on rate
                if(_context.Rates.Where(a=>a.CourseId==_courses.Id && a.RateType == 1).FirstOrDefault() != null)
                {
                    count++;
                    star += _context.Rates.Where(a => a.CourseId == _courses.Id && a.RateType==1).Average(a => a.Star);
                }               
            }
            //if don't exist rate is default is 5 star
            return star==0 ? 5 : (int)(star / count);
        }
        public IEnumerable<ReviewModel> GetReviewTeacher(int idCourses)
        {
            return _context.Rates.Include(o=>o.Account).Where(a => a.CourseId == idCourses && a.RateType == 1).Select(a => new ReviewModel
            {
                UserName = a.Account.FirstName + a.Account.LastName,
                Content = a.Content,
                Time = a.AddTime
            }).ToList();
        }
        public int GetRateCourses(int idCourses)
        {
            double star = 0;
           //check exist value on rate
           if (_context.Rates.Where(a => a.CourseId == idCourses && a.RateType == 2).FirstOrDefault() != null)
           {
                    star += _context.Rates.Where(a => a.CourseId == idCourses && a.RateType == 2).Average(a => a.Star);       
            }
            //if don't exist rate is default is 5 star
            return star == 0 ? 5 : (int)(star);
        }
        public IEnumerable<ReviewModel> GetReviewCourses(int idCourses)
        {
                return _context.Rates.Include(o=>o.Account).Where(a => a.CourseId == idCourses).Select(a => new ReviewModel {
                    UserName = a.Account.FirstName + " "+ a.Account.LastName,
                    Content=a.Content,
                    Time=a.AddTime
                 }).ToList();
        }
    }
}
