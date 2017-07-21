using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_ELearning.Models;
using Api_ELearning.Data;
using Microsoft.Extensions.Logging;
using Api_ELearning.ActionModels;
using Microsoft.EntityFrameworkCore;

namespace Api_ELearning.Repositories
{
    public class CoursesRepository : ICoursesRepository
    {

        private readonly ELearningDbContext _context;
        private readonly ILogger _log;
        private readonly IRateRepository _rate;

        public CoursesRepository(ELearningDbContext context, ILogger<CoursesRepository> log, IRateRepository rate)
        {
            _context = context;
            _log = log;
            _rate = rate;
        }
        public IEnumerable<Course> All
        {
            get
            {
                _log.LogInformation("GET => All");
                return _context.Courses.ToList();
            }
            set { }
        }

        public bool Add(Course o)
        {
            try
            {
                _log.LogInformation("BEGIN => Add");
                o.AddTime = DateTime.Now;
                o.EditTime = DateTime.Now;
                _context.Courses.Add(o);
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

        public bool Edit(Course o)
        {
            try
            {
                _log.LogInformation("BEGIN => Edit");
                o.EditTime = DateTime.Now;
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

        public Course Get(int id)
        {
            _log.LogInformation("GET => [%s]", id);
            return _context.Courses.Find(id);
        }

        public bool Remove(int id)
        {
            try
            {
                _log.LogInformation("BEGIN => Remove");
                _context.Courses.Remove(_context.Courses.Find(id));
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
        public IEnumerable<CoursesActionModel> ListCourse()
        {
            return _context.Courses.Include(o=>o.Category).Include(k=>k.Account).Include(j=>j.Cetificate).Select(a => new CoursesActionModel
            {
                Id = a.Id,
                NameCourses = a.Name,
                Donors = a.Donors,
                NumberCredits = a.NumberCredits,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                Description = a.Description,
                Price = a.Price,
                Category = a.Category.Name,
                UrlImage = a.UrlImage,
                Cetificate = a.Cetificate.Name,
                idCategory=a.Category.Id,
                idCetificate=a.Cetificate.Id,
                idTeacher=a.Account.Id,
                Teacher = a.Account.FirstName + a.Account.LastName,
                AddTime = a.AddTime,
                EditTime = a.EditTime
            }).ToList();
        }
        public IEnumerable<CoursesActionModel> ListCoursesTeacher(int idTeacher)
        {
            return _context.Courses.Include(o=>o.Category).Include(k=>k.Account).Where(a => a.AccountId == idTeacher).Select(a=> new CoursesActionModel {
                Id = a.Id,
                NameCourses = a.Name,
                Donors = a.Donors,
                NumberCredits = a.NumberCredits,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                Description = a.Description,
                Price = a.Price,
                Category = a.Category.Name,
                UrlImage = a.UrlImage,
                Cetificate = a.Cetificate.Name,
                idCategory = a.Category.Id,
                idCetificate = a.Cetificate.Id,
                idTeacher = a.Account.Id,
                Teacher = a.Account.FirstName + a.Account.LastName,
                AddTime = a.AddTime,
                EditTime = a.EditTime
            }).ToList();
        }
        public IEnumerable<CoursesActionModel> ListCoursesCategory(int idCategory)
        {
            return _context.Courses.Include(o=>o.Category).Include(k=>k.Account).Where(a => a.CategoryId == idCategory).Select(a => new CoursesActionModel
            {
                Id = a.Id,
                NameCourses = a.Name,
                Donors = a.Donors,
                NumberCredits = a.NumberCredits,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                Description = a.Description,
                Price = a.Price,
                Category = a.Category.Name,
                UrlImage = a.UrlImage,
                Cetificate = a.Cetificate.Name,
                idCategory = a.Category.Id,
                idCetificate = a.Cetificate.Id,
                idTeacher = a.Account.Id,
                Teacher = a.Account.FirstName + a.Account.LastName,
                AddTime = a.AddTime,
                EditTime = a.EditTime
            }).ToList();
        }
        public IEnumerable<CoursesActionModel> ListMostPopular(int numberCourses)
        {
            List<CoursesActionModel> listMost = new List<CoursesActionModel>();
            //get top lessons
            var listLesson = _context.LessonDetailses.GroupBy(o=>o.Lesson.CourseId)
                .Select(c=>new { CoursesId= c.Select(d=>d.Lesson.CourseId).FirstOrDefault(), Count = c.Count() }).ToList();
            listLesson = listLesson.OrderByDescending(o => o.Count).ToList();
            // add list courses to list result
            //Course a;
            foreach (var item in listLesson)
            {
                listMost.Add(_context.Courses.Include(o => o.Category).Include(k => k.Account).Where(a => a.Id == item.CoursesId).Select(a => new CoursesActionModel
                {
                    Id = a.Id,
                    NameCourses = a.Name,
                    Donors = a.Donors,
                    NumberCredits = a.NumberCredits,
                    DateStart = a.DateStart,
                    DateEnd = a.DateEnd,
                    Description = a.Description,
                    Price = a.Price,
                    Category = a.Category.Name,
                    UrlImage = a.UrlImage,
                    Cetificate = a.Cetificate.Name,
                    idCategory = a.Category.Id,
                    idCetificate = a.Cetificate.Id,
                    idTeacher = a.Account.Id,
                    Teacher = a.Account.FirstName + a.Account.LastName,
                    AddTime = a.AddTime,
                    EditTime = a.EditTime
                }).FirstOrDefault());               
            }
            return listMost.Take(numberCourses);
        }
        public IEnumerable<CoursesActionModel> ListNewCourses(int numberCourses)
        {
            return _context.Courses.Include(o=>o.Category).Include(k=>k.Account).OrderByDescending(a => a.AddTime).Take(numberCourses).Select(a => new CoursesActionModel
            {
                Id = a.Id,
                NameCourses = a.Name,
                Donors = a.Donors,
                NumberCredits = a.NumberCredits,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                Description = a.Description,
                Price = a.Price,
                Category = a.Category.Name,
                UrlImage = a.UrlImage,
                Cetificate = a.Cetificate.Name,
                idCategory = a.Category.Id,
                idCetificate = a.Cetificate.Id,
                idTeacher = a.Account.Id,
                Teacher = a.Account.FirstName + a.Account.LastName,
                AddTime = a.AddTime,
                EditTime = a.EditTime
            }).ToList();
        } 
        public IEnumerable<CoursesActionModel> ListTopReview(int numberCourses)
        {
            List<CoursesActionModel> listTopReview = new List<CoursesActionModel>();
            //get list id on table Complain
            var listCourses = _context.Rates.Where(result=>result.RateType==1).GroupBy(result => result.CourseId)
                .Select(result => new { CourseId = result.Key, Rate = result.Average(star => star.Star), }).ToList();
            // filter list listCourses
            listCourses = listCourses.OrderByDescending(o => o.Rate).ToList();
            Course a;
            foreach (var item in listCourses)
            {
                a = _context.Courses.Include(i => i.Category)
                    .Include(i => i.Cetificate).Include(i => i.Lessons)
                    .Include(i => i.Account).FirstOrDefault(k => k.Id == item.CourseId);
                listTopReview.Add(new CoursesActionModel
                {
                    Id = a.Id,
                    NameCourses = a.Name,
                    Donors = a.Donors,
                    NumberCredits = a.NumberCredits,
                    DateStart = a.DateStart,
                    DateEnd = a.DateEnd,
                    Description = a.Description,
                    Price = a.Price,
                    Category = a.Category.Name,
                    UrlImage = a.UrlImage,
                    Cetificate = a.Cetificate.Name,
                    idCategory = a.Category.Id,
                    idCetificate = a.Cetificate.Id,
                    idTeacher = a.Account.Id,
                    Teacher = a.Account.FirstName + a.Account.LastName,
                    AddTime = a.AddTime,
                    EditTime = a.EditTime
                });
            }
            return listTopReview.Take(numberCourses);
        }
        public CoursesActionModel GetDetailCourses(int idCourses)
        {
            return _context.Courses.Include(o=>o.Category).Include(k=>k.Account).Where(a => a.Id == idCourses).Select(a => new CoursesActionModel {
                Id = a.Id,
                NameCourses = a.Name,
                Donors = a.Donors,
                NumberCredits = a.NumberCredits,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                Description = a.Description,
                Price = a.Price,
                Category = a.Category.Name,
                UrlImage = a.UrlImage,
                Cetificate = a.Cetificate.Name,
                idCategory = a.Category.Id,
                idCetificate = a.Cetificate.Id,
                idTeacher = a.Account.Id,
                Teacher = a.Account.FirstName + a.Account.LastName,
                AddTime = a.AddTime,
                EditTime = a.EditTime
            }).FirstOrDefault();
        }
        public IEnumerable<CoursesActionModel> FilterPrice(decimal from, decimal to)
        {
            return _context.Courses.Include(o=>o.Category).Include(k=>k.Account).Where(a => a.Price >= from && a.Price <= to).Select(a => new CoursesActionModel
            {
                Id = a.Id,
                NameCourses = a.Name,
                Donors = a.Donors,
                NumberCredits = a.NumberCredits,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                Description = a.Description,
                Price = a.Price,
                Category = a.Category.Name,
                UrlImage = a.UrlImage,
                Cetificate = a.Cetificate.Name,
                idCategory = a.Category.Id,
                idCetificate = a.Cetificate.Id,
                idTeacher = a.Account.Id,
                Teacher = a.Account.FirstName + a.Account.LastName,
                AddTime = a.AddTime,
                EditTime = a.EditTime
            }).ToList();
        }
        public IEnumerable<CoursesActionModel> FilterRate(decimal from, decimal to)
        {
            List<CoursesActionModel> listFilter = new List<CoursesActionModel>();
            var listCourse = _context.Courses.ToList();
            foreach (var item in listCourse)
            {
                var rate = _rate.GetRateCourses(item.Id);
                if (_rate.GetRateCourses(item.Id)>=from && _rate.GetRateCourses(item.Id) <= to)
                {
                    listFilter.Add(_context.Courses.Include(o => o.Category).Include(k => k.Account).Where(a => a.Id == item.Id).Select(a => new CoursesActionModel
                    {
                        Id = a.Id,
                        NameCourses = a.Name,
                        Donors = a.Donors,
                        NumberCredits = a.NumberCredits,
                        DateStart = a.DateStart,
                        DateEnd = a.DateEnd,
                        Description = a.Description,
                        Price = a.Price,
                        Category = a.Category.Name,
                        UrlImage = a.UrlImage,
                        Cetificate = a.Cetificate.Name,
                        idCategory = a.Category.Id,
                        idCetificate = a.Cetificate.Id,
                        idTeacher = a.Account.Id,
                        Teacher = a.Account.FirstName + a.Account.LastName,
                        AddTime = a.AddTime,
                        EditTime = a.EditTime
                    }).First());
                }
            }
            return listFilter;
        }
        public IEnumerable<CoursesActionModel> FilterCategory(int idCategory)
        {         
            return _context.Courses.Where(a=>a.CategoryId==idCategory).Select(a=>new CoursesActionModel
            {
                Id = a.Id,
                NameCourses = a.Name,
                Donors = a.Donors,
                NumberCredits = a.NumberCredits,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                Description = a.Description,
                Price = a.Price,
                Category = a.Category.Name,
                UrlImage = a.UrlImage,
                Cetificate = a.Cetificate.Name,
                idCategory = a.Category.Id,
                idCetificate = a.Cetificate.Id,
                idTeacher = a.Account.Id,
                Teacher = a.Account.FirstName + a.Account.LastName,
                AddTime = a.AddTime,
                EditTime = a.EditTime
                }).ToList();
        }
        public IEnumerable<CoursesActionModel> Search(string search)
        {
            return _context.Courses.Include(o => o.Category).Include(k => k.Account).Where(a =>
             a.Name.Contains(@search) ||
                a.Description.Contains(@search) ||
                    a.Donors.Contains(@search)).Select(a => new CoursesActionModel
                {
                Id = a.Id,
                NameCourses = a.Name,
                Donors = a.Donors,
                NumberCredits = a.NumberCredits,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                Description = a.Description,
                Price = a.Price,
                Category = a.Category.Name,
                UrlImage = a.UrlImage,
                Cetificate = a.Cetificate.Name,
                idCategory = a.Category.Id,
                idCetificate = a.Cetificate.Id,
                idTeacher = a.Account.Id,
                Teacher = a.Account.FirstName + a.Account.LastName,
                AddTime = a.AddTime,
                EditTime = a.EditTime
            }).ToList();
        }

        public IEnumerable<CoursesActionModel> getListCoursePage(int numberItem, int indexPage)
        {
            //skip numberItem*(indexpage -1)
            //take numberItem
            return _context.Courses.Include(o => o.Category).Include(k => k.Account).OrderByDescending(a => a.AddTime).Skip(numberItem*(indexPage-1)).Take(numberItem).Select(a => new CoursesActionModel
            {
                Id = a.Id,
                NameCourses = a.Name,
                Donors = a.Donors,
                NumberCredits = a.NumberCredits,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                Description = a.Description,
                Price = a.Price,
                Category = a.Category.Name,
                UrlImage = a.UrlImage,
                Cetificate = a.Cetificate.Name,
                idCategory = a.Category.Id,
                idCetificate = a.Cetificate.Id,
                idTeacher = a.Account.Id,
                Teacher = a.Account.FirstName + a.Account.LastName,
                AddTime = a.AddTime,
                EditTime = a.EditTime
            }).ToList();
        }
    }
}
